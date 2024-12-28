using ClosedXML.Excel;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.Providers;
using static TeamUpWebScraperLibrary.ExcelSpreadsheetReport.ExcelSpreadSheetConstants;

namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport;


public class ExcelSpreadsheetReportProvider
{
	private readonly ILoggerAdapter<ExcelSpreadsheetReportProvider> _logger;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IXLWorkBookFactory _xlWorkBookFactory;

	public ExcelSpreadsheetReportProvider(
		ILoggerAdapter<ExcelSpreadsheetReportProvider> logger,
		IDateTimeProvider dateTimeProvider,
		IXLWorkBookFactory xlWorkBookFactory)
	{
		_logger = logger;
		_dateTimeProvider = dateTimeProvider;
		_xlWorkBookFactory = xlWorkBookFactory;
	}

	public string ExcelReportFileName
	{
		get
		{
			return string.Format(ExcelReportFileNameTemplate, _dateTimeProvider.DateTimeNow.ToString(ExcelReportFileNameDateFormat));
		}
	}

	public bool SaveExcelReport(string fileFullPath, List<EventSpreadSheetLine> reportSpreadsheetLines)
	{
		if (File.Exists(fileFullPath))
		{
			return false;
		}
		else
		{
			CreateNewExcelReportFile(fileFullPath, reportSpreadsheetLines);
			return true;
		}
	}

	private void CreateNewExcelReportFile(string filename, List<EventSpreadSheetLine> reportSpreadsheetLines)
	{
		using (var wb = _xlWorkBookFactory.CreateXLWorkBook())
		{
			var ws = wb.Worksheets.Add(ExcelReportSheetName);

			WriteHeaders(ws);

			// Write report lines
			WriteDataLinesToSpresdsheet(ws, ExcelReportHeaderLineNumber + 1, reportSpreadsheetLines);

			wb.SaveAs(filename);
		}
	}

	private void WriteHeaders(IXLWorksheet ws)
	{
		// Write all the Columns
		int columnIndex = (int)ExcelReportHeadersColumns.Id;
		ExcelReportHeaders.ForEach(h =>
		{
			ws.Cell(ExcelReportHeaderLineNumber, columnIndex).Value = h.Key;
			ws.Cell(ExcelReportHeaderLineNumber, columnIndex).WorksheetColumn().Width = h.Value;
			columnIndex++;
		});
	}

	private void WriteDataLinesToSpresdsheet(IXLWorksheet ws, int emptyRowNumber, List<EventSpreadSheetLine> reportSpreadsheetLines)
	{
		foreach (var line in reportSpreadsheetLines)
		{
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.Id).Value = line.Id;

			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.title).Value = line.Title;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.location).Value = line.Location;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.notes).Value = line.Notes;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.Start_Dt).Value = line.StartDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.End_Dt).Value = line.EndDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.Creation_Dt).Value = line.CreationDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.Update_Dt).Value = line.UpdateDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.Delete_Dt).Value = line.DeleteDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.SignupVisibility).Value = line.SignupVisibility;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.SignupCount).Value = line.SignupCount;

			int signupColumn = (int)ExcelReportHeadersColumns.Column1;
			// The Excell sheet has a limited number of columns
			// There fore we can only display in report ExcelReportSignupsLimit of signups
			foreach (var signup in line.Signups.Take(ExcelReportSignupsLimit).ToList())
			{
				ws.Cell(emptyRowNumber, signupColumn).Value = signup;
				signupColumn++;
			}

			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.Division_Column2).Value = line.Division;

			emptyRowNumber++;
		}
	}
}
