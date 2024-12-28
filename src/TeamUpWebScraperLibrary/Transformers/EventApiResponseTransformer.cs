using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebScraperLibrary.Transformers;

public static class EventApiResponseTransformer
{
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
		};
	}

	private static string GetEventId(Event eventData, List<CalendarConfiguration> calendarsMapping)
	{
		var eventDate = eventData.StartDate.ToString("yyMMdd");
		var eventTime = eventData.StartDate.ToString("HHmm");

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
