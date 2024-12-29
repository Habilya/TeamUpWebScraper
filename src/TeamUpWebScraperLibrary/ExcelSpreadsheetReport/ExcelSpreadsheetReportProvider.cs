using ClosedXML.Excel;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.Providers;
using static TeamUpWebScraperLibrary.ExcelSpreadsheetReport.ExcelSpreadSheetConstants;

namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport;


public class ExcelSpreadsheetReportProvider : IExcelSpreadsheetReportProvider
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
			return string.Format(EXCELREPORT_FILENAME_TEMPLATE, _dateTimeProvider.DateTimeNow.ToString(EXCELREPORT_FILENAME_DATE_FORMAT));
		}
	}

	public string ExcelReportDefaultSavePath
	{
		get
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + EXCELREPORT_USER_DEFAULT_FOLDER;
		}
	}

	public string ExcelReportSaveDialogFilter
	{
		get
		{
			return EXCELREPORT_SAVE_DIALOG_FILTER;
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
			var ws = wb.Worksheets.Add(EXCELREPORT_SHEET_NAME);

			WriteHeaders(ws);

			// Write report lines
			WriteDataLinesToSpresdsheet(ws, EXCELREPORT_HEADER_LINENUMBER + 1, reportSpreadsheetLines);

			wb.SaveAs(filename);
		}
	}

	private void WriteHeaders(IXLWorksheet ws)
	{
		// Write all the Columns
		int columnIndex = (int)ExcelReportHeadersColumns.Id;
		ExcelReportHeaders.ForEach(h =>
		{
			ws.Cell(EXCELREPORT_HEADER_LINENUMBER, columnIndex).Value = h.Key;
			ws.Cell(EXCELREPORT_HEADER_LINENUMBER, columnIndex).WorksheetColumn().Width = h.Value;
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
			foreach (var signup in line.Signups.Take(EXCELREPORT_SIGNUPS_LIMIT).ToList())
			{
				ws.Cell(emptyRowNumber, signupColumn).Value = signup;
				signupColumn++;
			}

			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_client2).Value = line.Client2;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_division).Value = line.Division;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_me_dical_medical).Value = line.Medical;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_priorite_priority2_1).Value = line.Priority;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_cate_gorie_category_1).Value = line.Category;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_responsable_in_charge).Value = line.ResponsibleInCharge;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_contrat_provincial_contract_1).Value = line.ProvincialContract;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_nombre_de_membres_ne_cessaires).Value = line.NbMembersNeeded;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.presences_collees).Value = line.PresencesConcat;


			emptyRowNumber++;
		}
	}
}
