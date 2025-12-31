namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

public class ExcelReportSpreadSheetConfig
{
	public string ReportSpreadSheetName { get; set; } = default!;
	public int ReportHeaderLine { get; set; } = default!;
	public int ReportSignupsLimit { get; set; } = default!;
	public string ReportHeaderBackgroundColorHtml { get; set; } = default!;
	public string ReportAttentionRequiredHighlightingColorHtml { get; set; } = default!;
	public string ReportDuplicateEventIdHighlightColorHtml { get; set; } = default!;
	public string EventTitlesToHighLightPattern { get; set; } = default!;
	public string FileNameTemplate { get; set; } = default!;
	public string FileNameDateTimeFormat { get; set; } = default!;
	public string SaveDefaultFolder { get; set; } = default!;
	public string SaveDialogFilter { get; set; } = default!;
}
