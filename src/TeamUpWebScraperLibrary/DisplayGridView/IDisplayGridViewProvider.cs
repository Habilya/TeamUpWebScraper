using TeamUpWebScraperLibrary.DTO;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

namespace TeamUpWebScraperLibrary.DisplayGridView;

public interface IDisplayGridViewProvider
{
	List<DisplayGridViewModel> TransformReportSpreadsheetLinesInotDisplayLines(List<EventSpreadSheetLine>? reportSpreadsheetLines);
}
