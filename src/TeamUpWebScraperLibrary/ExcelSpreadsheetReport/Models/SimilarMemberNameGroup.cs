namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

public class NameSimilarity
{
	public string Name1 { get; set; } = default!;
	public string Name2 { get; set; } = default!;
	public double Score { get; set; }
}

public class SimilarMemberNameGroup
{
	public List<MemberTimeAnalysisModel> Members { get; set; } = new();
	public List<NameSimilarity> Similarities { get; set; } = new();
}
