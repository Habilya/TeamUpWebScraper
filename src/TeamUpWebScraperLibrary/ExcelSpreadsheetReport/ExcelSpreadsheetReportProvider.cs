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

	public bool SaveExcelReport(string fileFullPath,
		List<EventSpreadSheetLine> reportSpreadsheetLines,
		bool isMemberTimeAnalysisIncluded,
		List<MemberTimeAnalysisModel> memberTimeAnalysisData,
		List<MemberTimeReportModel> memberTimeReportData)
	{
		if (File.Exists(fileFullPath))
		{
			return false;
		}
		else
		{
			CreateNewExcelReportFile(
					fileFullPath,
					reportSpreadsheetLines,
					isMemberTimeAnalysisIncluded,
					memberTimeAnalysisData,
					memberTimeReportData);

			return true;
		}
	}

	private void CreateNewExcelReportFile(string filename,
		List<EventSpreadSheetLine> reportSpreadsheetLines,
		bool isMemberTimeAnalysisIncluded,
		List<MemberTimeAnalysisModel> memberTimeAnalysisData,
		List<MemberTimeReportModel> memberTimeReportData)
	{
		using (var wb = _xlWorkBookFactory.CreateXLWorkBook())
		{
			var _xLWorksheet = wb.Worksheets.Add(_excelReportSpreadSheetConfig.ReportSpreadSheetName);

			WriteHeaders(_xLWorksheet);

			// Write report lines
			WriteDataLinesToSpresdsheet(_xLWorksheet, _excelReportSpreadSheetConfig.ReportHeaderLine + 1, reportSpreadsheetLines);

			if (isMemberTimeAnalysisIncluded)
			{
				var _xLWSTimeAnalysis = wb.Worksheets.Add("Time Analysis");
				WriteTimeAnalysisWorkSheet(wb, _xLWSTimeAnalysis, memberTimeAnalysisData);

				var _xLWSMemberTimeReport = wb.Worksheets.Add("Member Time Report");
				WriteMemberTimeReportWorkSheet(wb, _xLWSMemberTimeReport, memberTimeReportData);
			}

			wb.SaveAs(filename);
		}
	}

	private void WriteMemberTimeReportWorkSheet(IXLWorkbook wb, IXLWorksheet ws, List<MemberTimeReportModel> memberTimeReportData)
	{
		// Write all the Columns
		const int _headerLine = 1;
		int columnIndex = (int)ExcelReportMembersTimeHeadersColumns.Name;
		ExcelReportMembersTimeHeaders.ForEach(h =>
		{
			ws.Cell(_headerLine, columnIndex).Value = h.Key;
			ws.Cell(_headerLine, columnIndex).WorksheetColumn().Width = h.Value;
			ws.Cell(_headerLine, columnIndex).Style.Fill.BackgroundColor = GetXLColorFromHtmlColor(_excelReportSpreadSheetConfig.ReportHeaderBackgroundColorHtml);
			columnIndex++;
		});

		var emptyRowNumber = _headerLine + 1;
		foreach (var line in memberTimeReportData.OrderBy(o => o.SignupName))
		{
			ws.Cell(emptyRowNumber, (int)ExcelReportMembersTimeHeadersColumns.Name).Value = line.SignupName;
			ws.Cell(emptyRowNumber, (int)ExcelReportMembersTimeHeadersColumns.Hours).Value = line.Hours;
			ws.Cell(emptyRowNumber, (int)ExcelReportMembersTimeHeadersColumns.HoursPlus2).Value = line.HoursPlus2;
			ws.Cell(emptyRowNumber, (int)ExcelReportMembersTimeHeadersColumns.NBEvents).Value = line.NBEvents;
			ws.Cell(emptyRowNumber, (int)ExcelReportMembersTimeHeadersColumns.OtherNameOccurances).Value = line.OtherNameOccurances;

			emptyRowNumber++;
		}

		// Transform entire line set into a table
		var tableRange = ws.Range(_headerLine, (int)ExcelReportMembersTimeHeadersColumns.Name, emptyRowNumber - 1, (int)ExcelReportMembersTimeHeadersColumns.OtherNameOccurances);
		var table = tableRange.CreateTable();
	}

	private void WriteTimeAnalysisWorkSheet(IXLWorkbook wb, IXLWorksheet ws, List<MemberTimeAnalysisModel> memberTimeAnalysisData)
	{
		// Write all the Columns
		const int _headerLine = 1;
		int columnIndex = (int)ExcelReportSignupsAnalysisHeadersColumns.SignupName;
		ExcelReportSignupsAnalysisHeaders.ForEach(h =>
		{
			ws.Cell(_headerLine, columnIndex).Value = h.Key;
			ws.Cell(_headerLine, columnIndex).WorksheetColumn().Width = h.Value;
			ws.Cell(_headerLine, columnIndex).Style.Fill.BackgroundColor = GetXLColorFromHtmlColor(_excelReportSpreadSheetConfig.ReportHeaderBackgroundColorHtml);
			columnIndex++;
		});

		var emptyRowNumber = _headerLine + 1;
		foreach (var line in memberTimeAnalysisData)
		{
			ws.Cell(emptyRowNumber, (int)ExcelReportSignupsAnalysisHeadersColumns.SignupName).Value = line.SignupName;
			ws.Cell(emptyRowNumber, (int)ExcelReportSignupsAnalysisHeadersColumns.Hours).Value = line.Hours;
			ws.Cell(emptyRowNumber, (int)ExcelReportSignupsAnalysisHeadersColumns.HoursPlus2).Value = line.HoursPlus2;
			ws.Cell(emptyRowNumber, (int)ExcelReportSignupsAnalysisHeadersColumns.Event).Value = line.EventName;
			ws.Cell(emptyRowNumber, (int)ExcelReportSignupsAnalysisHeadersColumns.Start_Dt).Value = line.StartDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportSignupsAnalysisHeadersColumns.End_Dt).Value = line.EndDate;

			emptyRowNumber++;
		}

		// Transform entire line set into a table
		ws.Range(_headerLine, (int)ExcelReportSignupsAnalysisHeadersColumns.SignupName, emptyRowNumber - 1, (int)ExcelReportSignupsAnalysisHeadersColumns.End_Dt)
			.CreateTable();
	}

	private void WriteHeaders(IXLWorksheet ws)
	{
		// Write all the Columns
		int columnIndex = (int)ExcelReportEventHeadersColumns.Id;
		ExcelReportEventsHeaders.ForEach(h =>
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
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.Id).Value = line.EventId;

			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.title).Value = line.Title;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.location).Value = line.Location;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.notes).Value = line.Notes;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.Start_Dt).Value = line.StartDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.End_Dt).Value = line.EndDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.Creation_Dt).Value = line.CreationDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.Update_Dt).Value = line.UpdateDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.Delete_Dt).Value = line.DeleteDate;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.SignupVisibility).Value = line.SignupVisibility;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.SignupCount).Value = line.SignupCount;

			int signupColumn = (int)ExcelReportEventHeadersColumns.Column1;
			// The Excell sheet has a limited number of columns
			// There fore we can only display in report ExcelReportSignupsLimit of signups
			foreach (var signup in line.Signups.Take(_excelReportSpreadSheetConfig.ReportSignupsLimit).ToList())
			{
				ws.Cell(emptyRowNumber, signupColumn).Value = signup;
				signupColumn++;
			}

			var events_custom_client2 = ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.events_custom_client2);
			events_custom_client2.Value = line.Client2;
			events_custom_client2.Style.NumberFormat.Format = "@";

			var events_custom_division = ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.events_custom_division);
			events_custom_division.Value = line.Division;
			events_custom_division.Style.NumberFormat.Format = "@";

			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.events_custom_me_dical_medical).Value = line.Medical;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.events_custom_priorite_priority2_1).Value = line.Priority;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.events_custom_cate_gorie_category_1).Value = line.Category;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.events_custom_responsable_in_charge).Value = line.ResponsibleInCharge;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.events_custom_contrat_provincial_contract_1).Value = line.ProvincialContract;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.events_custom_nombre_de_membres_ne_cessaires).Value = line.NbMembersNeeded;
			ws.Cell(emptyRowNumber, (int)ExcelReportEventHeadersColumns.presences_collees).Value = line.PresencesConcat;

			ManageLineHighLighting(ws, emptyRowNumber, line.LineHighLightColor);

			emptyRowNumber++;
		}

		// Transform entire line set into a table
		ws.Range(_excelReportSpreadSheetConfig.ReportHeaderLine, (int)ExcelReportEventHeadersColumns.Id, emptyRowNumber - 1, (int)ExcelReportEventHeadersColumns.presences_collees)
			.CreateTable();
	}

	private void ManageLineHighLighting(IXLWorksheet ws, int emptyRowNumber, string LineHighLightColor)
	{
		var lineHighlightXLColor = GetXLColorFromHtmlColor(LineHighLightColor);

		if (lineHighlightXLColor == XLColor.Transparent)
		{
			return;
		}

		ws.Range(emptyRowNumber, (int)ExcelReportEventHeadersColumns.Id, emptyRowNumber, (int)ExcelReportEventHeadersColumns.presences_collees)
				.Style.Fill.BackgroundColor = lineHighlightXLColor;
	}
}
