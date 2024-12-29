using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebScraperLibrary.Transformers;

public class EventApiResponseTransformer : IEventApiResponseTransformer
{
	public const string STRING_DATE_TIME_FORMAT = "yyyy-MM-dd";
	public const string EVENT_ID_EVENT_DATE_FORMAT = "yyMMdd";
	public const string EVENT_ID_EVENT_TIME_FORMAT = "HHmm";

	public const string EVENT_ID_PREFIX_CB = "CB";
	public const string EVENT_ID_PREFIX_PB = "PB";
	public const string EVENT_ID_PREFIX_DIV = "DIV";

	public const string CENTRE_BELL_SEARCHABLE = "centre bell";
	public const string PLACE_BELL_SEARCHABLE = "pb";

	public EventApiResponseTransformer()
	{

	}

	public List<EventSpreadSheetLine> EventApiResponseToSpreadSheetLines(List<Event> events, List<CalendarConfiguration> calendarsMapping)
	{
		var eventSpreadSheetLines = new List<EventSpreadSheetLine>();

		foreach (var eventData in events.Where(q => q.SignupCount > 0))
		{
			eventSpreadSheetLines.Add(SingleEventResponseToSpreadSheetLine(eventData, calendarsMapping));
		}

		return eventSpreadSheetLines;
	}

	private EventSpreadSheetLine SingleEventResponseToSpreadSheetLine(Event eventData, List<CalendarConfiguration> calendarsMapping)
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
			UpdateDate = eventData.UpdateDate?.ToString(STRING_DATE_TIME_FORMAT) ?? default!,
			DeleteDate = eventData.DeleteDate?.ToString(STRING_DATE_TIME_FORMAT) ?? default!,
			SignupVisibility = eventData.SignupVisibility,
			SignupCount = eventData.SignupCount.ToString(),
			Signups = GetSignups(eventData),

			Client2 = eventData.Custom.Client2,
			Division = GetDivision(eventData, calendarsMapping),
			Medical = eventData.Custom.MeDicalMedical,
			Priority = GetConcatenatedListValues(eventData.Custom.PrioritePriority2),
			Category = GetConcatenatedListValues(eventData.Custom.CateGorieCategory),
			ResponsibleInCharge = eventData.Custom.ResponsableInCharge,
			ProvincialContract = GetConcatenatedListValues(eventData.Custom.ContratProvincialContract),
			NbMembersNeeded = eventData.Custom.NombreDeMembresNecessaires,

			PresencesConcat = GetPresencesConcat(eventData)
		};
	}

	private string? GetConcatenatedListValues(List<string>? listFromCustom)
	{
		if (listFromCustom is null || !listFromCustom.Any())
		{
			return default;
		}

		return string.Join(", ", listFromCustom.ToArray());
	}

	private string GetPresencesConcat(Event eventData)
	{
		if (eventData.Signups is null || !eventData.Signups.Any())
		{
			return string.Empty;
		}

		return string.Join(" //", eventData.Signups!.Select(q => q.Name).ToArray());
	}

	private List<string> GetSignups(Event eventData)
	{
		return eventData.Signups?.Select(q => q.Name).ToList() ?? new List<string>();
	}

	private string GetEventId(Event eventData, List<CalendarConfiguration> calendarsMapping)
	{
		var eventDate = eventData.StartDate.ToString(EVENT_ID_EVENT_DATE_FORMAT);
		var eventTime = eventData.StartDate.ToString(EVENT_ID_EVENT_TIME_FORMAT);

		return $"{GetEventIdPrefix(eventData, calendarsMapping)}-{eventDate}-{eventTime}-{GetDivision(eventData, calendarsMapping)}";
	}

	private string GetDivision(Event eventData, List<CalendarConfiguration> calendarsMapping)
	{
		var prefix = GetEventIdPrefix(eventData, calendarsMapping);

		if (prefix.Equals(EVENT_ID_PREFIX_CB))
		{
			return "0452";
		}
		else if (prefix.Equals(EVENT_ID_PREFIX_PB))
		{
			return "0971";
		}
		else
		{
			var subCalendarNames = GetSubCalendarsOfEvent(eventData, calendarsMapping);
			string subCalendarName = "";
			if (!subCalendarNames.Any())
			{
				return "";
			}
			else if (subCalendarNames.Count > 1)
			{
				subCalendarName = subCalendarNames[1];
			}
			else
			{
				subCalendarName = subCalendarNames.First();
			}

			var afterDelimiter = GetStringAfter(subCalendarName, "> ");
			if (afterDelimiter.StartsWith("D") && afterDelimiter.Length > 1)
			{
				// If it starts with "D" and has more than 1 character, return the last 4 digits
				string numberPart = new string(afterDelimiter.Skip(1).TakeWhile(char.IsDigit).ToArray());
				return numberPart.Length > 4 ? numberPart.Substring(0, 4) : numberPart.PadLeft(4, '0');
			}
			else
			{
				// Otherwise, return the first 4 characters of the string (for words like "Prov" or "Gard")
				return afterDelimiter.Length >= 4 ? afterDelimiter.Substring(0, 4) : afterDelimiter;
			}
		}
	}

	private string GetEventIdPrefix(Event eventData, List<CalendarConfiguration> calendarsMapping)
	{
		var subCalendarNames = GetSubCalendarsOfEvent(eventData, calendarsMapping);

		if (subCalendarNames.Where(q => q.Contains(CENTRE_BELL_SEARCHABLE, StringComparison.OrdinalIgnoreCase)).Any())
		{
			return EVENT_ID_PREFIX_CB;
		}

		if (eventData.Title.Contains(PLACE_BELL_SEARCHABLE, StringComparison.OrdinalIgnoreCase))
		{
			return EVENT_ID_PREFIX_PB;
		}

		return EVENT_ID_PREFIX_DIV;
	}

	private List<string> GetSubCalendarsOfEvent(Event eventData, List<CalendarConfiguration> calendarsMapping)
	{
		List<string> subCalendarNames = new List<string>();
		if (eventData.SubcalendarIds.Any())
		{
			subCalendarNames = calendarsMapping
				.Where(q => eventData.SubcalendarIds.Contains(q.Id))
				.Select(q => q.Name)
				.ToList();
		}

		return subCalendarNames;
	}

	private string GetStringAfter(string input, string delimiter)
	{
		int index = input.IndexOf(delimiter);

		if (index != -1)
		{
			return input.Substring(index + delimiter.Length);
		}

		return string.Empty;
	}
}
