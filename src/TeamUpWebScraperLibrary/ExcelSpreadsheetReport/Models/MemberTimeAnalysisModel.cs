namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

public class MemberTimeReportModel
{
	public string SignupName { get; set; } = default!;
	public double Hours { get; set; } = default!;
	public double HoursPlus2 { get; set; } = default!;
	public string Qualtification { get; set; } = default!;
	public string Division { get; set; } = default!;
	public string CONT { get; set; } = default!;
	public int NBEvents { get; set; } = default!;
	public string OtherNameOccurances { get; set; } = default!;
}
