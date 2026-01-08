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
			var resultLine = new DisplayGridViewModel()
			{
				Selected = true,
				UniqueId = reportLine.UniqueId,
				Title = reportLine.Title,
				EventId = reportLine.EventId,
				StartDate = reportLine.StartDate,
				EndDate = reportLine.EndDate,
				Hours = reportLine.Hours,
				SignupCount = reportLine.SignupCount,
				PresencesConcat = reportLine.PresencesConcat
			};

			result.Add(resultLine);
		}

		return result;
	}
}
