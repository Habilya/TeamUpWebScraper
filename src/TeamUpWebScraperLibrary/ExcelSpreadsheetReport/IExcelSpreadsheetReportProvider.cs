using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport;

public interface IExcelSpreadsheetReportProvider
{
	string ExcelReportFileName { get; }

	string ExcelReportDefaultSavePath { get; }

	string ExcelReportSaveDialogFilter { get; }

	bool SaveExcelReport(string fileFullPath, List<EventSpreadSheetLine> reportSpreadsheetLines);
}
