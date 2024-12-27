namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

public class EventSpreadSheetLine
{
	public string Id { get; set; } = default!;

	public string Title { get; set; } = default!;
	public string Location { get; set; } = default!;
	public string Notes { get; set; } = default!;
	public string StartDate { get; set; } = default!;
	public string EndDate { get; set; } = default!;
	public string CreationDate { get; set; } = default!;
	public string UpdateDate { get; set; } = default!;
	public string DeleteDate { get; set; } = default!;
	public string SignupVisibility { get; set; } = default!;
	public string SignupCount { get; set; } = default!;

	public List<string> Signups { get; set; } = default!;

	public string Division { get; set; } = default!;
}
