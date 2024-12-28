using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport;

public interface IExcelSpreadsheetReportProvider
{
	string ExcelReportFileName { get; }

	bool SaveExcelReport(string fileFullPath, List<EventSpreadSheetLine> reportSpreadsheetLines);
}
