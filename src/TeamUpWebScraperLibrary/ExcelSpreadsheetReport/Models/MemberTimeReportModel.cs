namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

public class MemberTimeAnalysisModel
{
	public string SignupName { get; set; } = default!;
	public double Hours { get; set; } = default!;
	public double HoursPlus2 { get; set; } = default!;
	public string EventName { get; set; } = default!;
	public string StartDate { get; set; } = default!;
	public string EndDate { get; set; } = default!;
}
