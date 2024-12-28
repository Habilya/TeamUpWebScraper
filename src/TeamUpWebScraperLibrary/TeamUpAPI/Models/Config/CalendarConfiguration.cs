using System.Text.Json.Serialization;

namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;

public class CalendarConfiguration
{
	[JsonPropertyName("id")]
	public long Id { get; set; } = default!;

	[JsonPropertyName("name")]
	public string Name { get; set; } = default!;

	[JsonPropertyName("active")]
	public bool Active { get; set; } = default!;

	[JsonPropertyName("color")]
	public int Color { get; set; } = default!;

	[JsonPropertyName("overlap")]
	public bool Overlap { get; set; } = default!;

	[JsonPropertyName("readonly")]
	public bool Readonly { get; set; } = default!;

	[JsonPropertyName("creation_dt")]
	public DateTimeOffset CreationDate { get; set; } = default!;

	[JsonPropertyName("update_dt")]
	public DateTimeOffset? UpdateDate { get; set; } = default!;

	[JsonPropertyName("eventidPrefix")]
	public string EventidPrefix { get; set; } = default!;

	[JsonPropertyName("div")]
	public string Div { get; set; } = default!;
}
