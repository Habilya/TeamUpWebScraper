namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;

public class TeamUpApiConfiguration
{
	public string TeamupToken { get; set; } = default!;
	public string BaseURL { get; set; } = default!;
	public string CalendarId { get; set; } = default!;
	public string TimeZone { get; set; } = default!;
	public List<CalendarConfiguration> Calendars { get; set; } = default!;
}
