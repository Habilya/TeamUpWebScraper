using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebScraperLibrary.Transformers;

public static class EventApiResponseTransformer
{
	public const string STRING_DATE_TIME_FORMAT = "yyyy-MM-dd";
	public const string EVENT_ID_EVENT_DATE_FORMAT = "yyMMdd";
	public const string EVENT_ID_EVENT_TIME_FORMAT = "HHmm";

	public static List<EventSpreadSheetLine> EventApiResponseToSpreadSheetLines(List<Event> events, List<CalendarConfiguration> calendarsMapping)
	{
		var eventSpreadSheetLines = new List<EventSpreadSheetLine>();

		foreach (var eventData in events)
		{
			eventSpreadSheetLines.Add(SingleEventResponseToSpreadSheetLine(eventData, calendarsMapping));
		}

		return eventSpreadSheetLines;
	}

	private static EventSpreadSheetLine SingleEventResponseToSpreadSheetLine(Event eventData, List<CalendarConfiguration> calendarsMapping)
	{
		return new EventSpreadSheetLine
		{
			Id = GetEventId(eventData, calendarsMapping),
			Title = eventData.Title,
			Location = eventData.Location,
			Notes = eventData.Notes,
			StartDate = eventData.StartDate.ToString(STRING_DATE_TIME_FORMAT),
			EndDate = eventData.EndDate.ToString(STRING_DATE_TIME_FORMAT),
			CreationDate = eventData.CreationDate.ToString(STRING_DATE_TIME_FORMAT),
			UpdateDate = eventData.UpdateDate.ToString(STRING_DATE_TIME_FORMAT),
			DeleteDate = eventData.DeleteDate?.ToString(STRING_DATE_TIME_FORMAT) ?? default!,
			SignupVisibility = eventData.SignupVisibility,
			SignupCount = eventData.SignupCount.ToString(),
			Signups = GetSignups(eventData),
			Division = GetDivision(eventData)
		};
	}

	private static string GetDivision(Event eventData)
	{
		// TODO:
		var mainSubCalendarId = eventData.SubcalendarId;
		var customFieldDivision = eventData.Custom?.Division;
		return "";
	}

	private static List<string> GetSignups(Event eventData)
	{
		return eventData.Signups?.Select(q => q.Name).ToList() ?? new List<string>();
	}

	private static string GetEventId(Event eventData, List<CalendarConfiguration> calendarsMapping)
	{
		var eventDate = eventData.StartDate.ToString(EVENT_ID_EVENT_DATE_FORMAT);
		var eventTime = eventData.StartDate.ToString(EVENT_ID_EVENT_TIME_FORMAT);

		string eventPrefix = "";
		string eventSuffix = "";
		var correspondigCalendar = calendarsMapping
			.Where(q => q.Id.Equals(eventData.SubcalendarId))
			.FirstOrDefault();

		if (correspondigCalendar is not null)
		{
			eventPrefix = correspondigCalendar.EventidPrefix;
			eventSuffix = correspondigCalendar.Div;
		}

		return $"{eventPrefix}-{eventDate}-{eventTime}-{eventSuffix}";
	}
}
