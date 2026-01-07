using ClosedXML.Excel;
using System.Diagnostics;
using System.Drawing;
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
	private readonly ExcelReportSpreadSheetConfig _excelReportSpreadSheetConfig;

	public ExcelSpreadsheetReportProvider(
		ILoggerAdapter<ExcelSpreadsheetReportProvider> logger,
		IDateTimeProvider dateTimeProvider,
		IXLWorkBookFactory xlWorkBookFactory,
		ExcelReportSpreadSheetConfig excelReportSpreadSheetConfig)
	{
		_logger = logger;
		_dateTimeProvider = dateTimeProvider;
		_xlWorkBookFactory = xlWorkBookFactory;
		_excelReportSpreadSheetConfig = excelReportSpreadSheetConfig;
	}

	public XLColor GetXLColorFromHtmlColor(string htmlColor)
	{
		if (string.IsNullOrWhiteSpace(htmlColor))
		{
			return XLColor.Transparent;
		}

		try
		{
			return XLColor.FromColor(ColorTranslator.FromHtml(htmlColor));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Demystify(), $"Trouble parsing color {htmlColor} into XLcolor object");
			return XLColor.Transparent;
		}
	}

	public string ExcelReportFileName
	{
		get
		{
			return string.Format(_excelReportSpreadSheetConfig.FileNameTemplate, _dateTimeProvider.DateTimeNow.ToString(_excelReportSpreadSheetConfig.FileNameDateTimeFormat));
		}
	}

	public string ExcelReportDefaultSavePath
	{
		get
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + _excelReportSpreadSheetConfig.SaveDefaultFolder;
		}
	}

	public string ExcelReportSaveDialogFilter
	{
		get
		{
			return _excelReportSpreadSheetConfig.SaveDialogFilter;
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
			var _xLWorksheet = wb.Worksheets.Add(_excelReportSpreadSheetConfig.ReportSpreadSheetName);

			WriteHeaders(_xLWorksheet);

			// Write report lines
			WriteDataLinesToSpresdsheet(_xLWorksheet, _excelReportSpreadSheetConfig.ReportHeaderLine + 1, reportSpreadsheetLines);

			wb.SaveAs(filename);
		}
	}

	private void WriteHeaders(IXLWorksheet ws)
	{
		// Write all the Columns
		int columnIndex = (int)ExcelReportHeadersColumns.Id;
		ExcelReportHeaders.ForEach(h =>
		{
			ws.Cell(_excelReportSpreadSheetConfig.ReportHeaderLine, columnIndex).Value = h.Key;
			ws.Cell(_excelReportSpreadSheetConfig.ReportHeaderLine, columnIndex).WorksheetColumn().Width = h.Value;
			ws.Cell(_excelReportSpreadSheetConfig.ReportHeaderLine, columnIndex).Style.Fill.BackgroundColor = GetXLColorFromHtmlColor(_excelReportSpreadSheetConfig.ReportHeaderBackgroundColorHtml);
			columnIndex++;
		});
	}

	private void WriteDataLinesToSpresdsheet(IXLWorksheet ws, int emptyRowNumber, List<EventSpreadSheetLine> reportSpreadsheetLines)
	{
		foreach (var line in reportSpreadsheetLines)
		{
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.Id).Value = line.EventId;

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
			foreach (var signup in line.Signups.Take(_excelReportSpreadSheetConfig.ReportSignupsLimit).ToList())
			{
				ws.Cell(emptyRowNumber, signupColumn).Value = signup;
				signupColumn++;
			}

			var events_custom_client2 = ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_client2);
			events_custom_client2.Value = line.Client2;
			events_custom_client2.Style.NumberFormat.Format = "@";

			var events_custom_division = ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_division);
			events_custom_division.Value = line.Division;
			events_custom_division.Style.NumberFormat.Format = "@";

			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_me_dical_medical).Value = line.Medical;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_priorite_priority2_1).Value = line.Priority;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_cate_gorie_category_1).Value = line.Category;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_responsable_in_charge).Value = line.ResponsibleInCharge;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_contrat_provincial_contract_1).Value = line.ProvincialContract;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.events_custom_nombre_de_membres_ne_cessaires).Value = line.NbMembersNeeded;
			ws.Cell(emptyRowNumber, (int)ExcelReportHeadersColumns.presences_collees).Value = line.PresencesConcat;

			ManageLineHighLighting(ws, emptyRowNumber, line.LineHighLightColor);

			emptyRowNumber++;
		}

		// Transform entire line set into a table
		ws.Range(_excelReportSpreadSheetConfig.ReportHeaderLine, (int)ExcelReportHeadersColumns.Id, emptyRowNumber - 1, (int)ExcelReportHeadersColumns.presences_collees)
			.CreateTable();
	}

	private void ManageLineHighLighting(IXLWorksheet ws, int emptyRowNumber, string LineHighLightColor)
	{
		var lineHighlightXLColor = GetXLColorFromHtmlColor(LineHighLightColor);

		if (lineHighlightXLColor == XLColor.Transparent)
		{
			return;
		}

		ws.Range(emptyRowNumber, (int)ExcelReportHeadersColumns.Id, emptyRowNumber, (int)ExcelReportHeadersColumns.presences_collees)
				.Style.Fill.BackgroundColor = lineHighlightXLColor;
	}
}
