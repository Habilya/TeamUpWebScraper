namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;

public class CalendarConfiguration
{
	public long Id { get; set; } = default!;
	public string Name { get; set; } = default!;
	public bool Active { get; set; } = default!;
	public int Color { get; set; } = default!;
	public bool Overlap { get; set; } = default!;
	public bool Readonly { get; set; } = default!;
	public DateTime CreationDt { get; set; } = default!;
	public DateTime UpdateDt { get; set; } = default!;
}
