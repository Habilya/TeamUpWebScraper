using System.Text.Json.Serialization;

namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;
/// <summary>
/// There are more fields, but i'm parsing only what is somewhat needed
/// https://teamup.stoplight.io/docs/api/046361930f27a-get-a-collection-of-sub-calendars
/// </summary>
public class Subcalendar
{
	public long Id { get; set; } = default!;

	public string Name { get; set; } = default!;

	public bool Active { get; set; } = default!;

	/// <summary>
	/// The color id references one of the 48 colors.
	/// Default: 17
	/// See this reference list for an overview of supported colors.
	/// https://teamup.stoplight.io/docs/api/ZG9jOjI4Mzk0ODA5-colors
	/// </summary>
	public int Color { get; set; } = default!;

	/// <summary>
	/// Flag that indicates if overlapping events are allowed or not for this sub-calendar.
	/// Default: true
	/// </summary>
	public bool Overlap { get; set; } = default!;

	/// <summary>
	/// Two types of sub-calendars are supported. 
	/// If type = 1, the sub-calendar is automatically fed by an iCalendar feed and is read-only. 
	/// If type = 0, the sub-calendar is a standard sub-calendars that can be used to read/write events.
	/// The type attribute is returned only if the calendar key used to access the calendar has administration permission.
	/// Default: 0
	/// </summary>
	public int Type { get; set; } = default!;

	/// <summary>
	/// This is a calculated field, not a field stored in the database.
	/// Default: false
	/// </summary>
	public bool Readonly { get; set; } = default!;

	/// <summary>
	/// Date and time when sub-calendar was created.
	/// Example: 2021-02-23T12:03:07+00:00
	/// </summary>
	[JsonPropertyName("creation_dt")]
	public DateTimeOffset CreationDate { get; set; } = default!;

	/// <summary>
	/// Date and time when sub-calendar was last updated.
	/// Example: null
	/// </summary>
	[JsonPropertyName("update_dt")]
	public DateTimeOffset? UpdateDate { get; set; } = default!;
}
