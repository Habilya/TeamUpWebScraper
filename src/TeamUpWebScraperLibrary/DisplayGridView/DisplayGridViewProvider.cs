using TeamUpWebScraperLibrary.DTO;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

namespace TeamUpWebScraperLibrary.DisplayGridView;


public class DisplayGridViewProvider : IDisplayGridViewProvider
{
	public List<DisplayGridViewModel> TransformReportSpreadsheetLinesInotDisplayLines(List<EventSpreadSheetLine>? reportSpreadsheetLines)
	{
		if (reportSpreadsheetLines is null || !reportSpreadsheetLines.Any())
		{
			return new List<DisplayGridViewModel>();
		}

		var result = new List<DisplayGridViewModel>();

		foreach (var reportLine in reportSpreadsheetLines)
		{
			var resultLine = new DisplayGridViewModel(
				reportLine.Title,
				reportLine.EventId,
				reportLine.StartDate,
				reportLine.EndDate,
				reportLine.Hours,
				reportLine.SignupCount,
				reportLine.PresencesConcat
			);

			result.Add(resultLine);
		}

		return result;
	}
}
