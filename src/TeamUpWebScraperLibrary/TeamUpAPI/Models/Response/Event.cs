using System.Text.Json.Serialization;

namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

/// <summary>
/// There are more fields, but i'm parsing only what is somewhat needed
/// https://teamup.stoplight.io/docs/api/0f9f896800ffe-get-events-collection
/// </summary>
public class Event
{
	/// <summary>
	/// The event-identifier is an integer for a non-recurring event. 
	/// For recurring events we will return the individual instances according to the recurrence rule and possibly existing exceptions. 
	/// The individual instances will have an identifier that consists of the event-ID followed by '-rid-' and the unix-timestamp of the start-time of the individual event.
	/// So an instance of the recurring event 123 starting on Thu Apr 04 15:43:58 2024 UTC would have the ID 123-rid-1712245438
	/// </summary>
	public string Id { get; set; } = default!;
	[JsonPropertyName("subcalendar_id")]
	public long SubcalendarId { get; set; } = default!;
	[JsonPropertyName("subcalendar_ids")]
	public List<long> SubcalendarIds { get; set; } = default!;
	public string Title { get; set; } = default!;
	public string Location { get; set; } = default!;
	public string Notes { get; set; } = default!;
	[JsonPropertyName("start_dt")]
	public DateTimeOffset StartDate { get; set; } = default!;
	[JsonPropertyName("end_dt")]
	public DateTimeOffset EndDate { get; set; } = default!;
	[JsonPropertyName("creation_dt")]
	public DateTimeOffset CreationDate { get; set; } = default!;
	[JsonPropertyName("update_dt")]
	public DateTimeOffset? UpdateDate { get; set; } = default!;
	[JsonPropertyName("delete_dt")]
	public DateTimeOffset? DeleteDate { get; set; } = default!;
	public string SignupVisibility { get; set; } = default!;
	[JsonPropertyName("signup_count")]
	public int SignupCount { get; set; } = default!;
	public List<Signup>? Signups { get; set; } = default!;
	public Custom? Custom { get; set; } = default!;


	#region !! Those fields are not used, just left here for a reference !!

	//public string SeriesId { get; set; }
	//public string RemoteId { get; set; }
	//public bool CommentsEnabled { get; set; }
	//public string CommentsVisibility { get; set; }
	//public List<object> Comments { get; set; }
	//public DateTime? RiStartDt { get; set; }
	//public DateTime? RsStartDt { get; set; }
	//public bool SignupEnabled { get; set; }
	//public int SignupLimit { get; set; }
	//public DateTime SignupDeadline { get; set; }
	//public bool SignupDeadlineEnabled { get; set; }
	//public string Who { get; set; }
	//public string Version { get; set; }
	//public bool Readonly { get; set; }
	//public string Tz { get; set; }
	//public List<object> Attachments { get; set; }
	//public bool AllDay { get; set; }
	//public string RRule { get; set; }
	#endregion
}
