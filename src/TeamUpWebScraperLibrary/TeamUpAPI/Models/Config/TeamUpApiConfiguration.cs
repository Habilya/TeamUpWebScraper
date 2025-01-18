namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;

public class TeamUpApiConfiguration
{
	public string TeamupToken { get; set; } = default!;
	public string BaseURL { get; set; } = default!;
	public string CalendarId { get; set; } = default!;
	public string TimeZone { get; set; } = default!;
	public int MaxDaysDataSpanLimit { get; set; } = default!;
	public int MaxHttpCallRetries { get; set; } = default!;
}
