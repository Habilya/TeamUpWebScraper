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
			var resultLine = new DisplayGridViewModel();

			resultLine.Id = reportLine.Id;
			resultLine.Title = reportLine.Title;
			resultLine.StartDate = reportLine.StartDate;
			resultLine.EndDate = reportLine.EndDate;
			resultLine.SignupCount = reportLine.SignupCount;
			resultLine.PresencesConcat = reportLine.PresencesConcat;

			result.Add(resultLine);
		}

		return result;
	}
}
