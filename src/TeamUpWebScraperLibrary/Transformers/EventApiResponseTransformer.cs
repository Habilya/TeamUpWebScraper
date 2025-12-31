using HtmlAgilityPack;
using System.Text.RegularExpressions;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebScraperLibrary.Transformers;

public class EventApiResponseTransformer : IEventApiResponseTransformer
{
	public const string STRING_DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";
	public const string EVENT_ID_EVENT_DATE_FORMAT = "yyMMdd";
	public const string EVENT_ID_EVENT_TIME_FORMAT = "HHmm";

	public const string EVENT_ID_PREFIX_CB = "CB";
	public const string EVENT_ID_PREFIX_PB = "PB";
	public const string EVENT_ID_PREFIX_DIV = "DIV";

	public const string CENTRE_BELL_SEARCHABLE = "centre bell";
	public const string PLACE_BELL_SEARCHABLE = "pb";


	private readonly ExcelReportSpreadSheetConfig _excelReportSpreadSheetConfig;
	private readonly Regex? _highlightTitlePattern;

	public EventApiResponseTransformer(ExcelReportSpreadSheetConfig excelReportSpreadSheetConfig)
	{
		_excelReportSpreadSheetConfig = excelReportSpreadSheetConfig;

		if (!string.IsNullOrWhiteSpace(_excelReportSpreadSheetConfig.EventTitlesToHighLightPattern))
		{
			_highlightTitlePattern = new Regex(_excelReportSpreadSheetConfig.EventTitlesToHighLightPattern, RegexOptions.IgnoreCase);
		}
	}

	public List<EventSpreadSheetLine> EventApiResponseToSpreadSheetLines(List<Event> events, List<Subcalendar> calendarsMapping)
	{
		var eventSpreadSheetLines = new List<EventSpreadSheetLine>();

		foreach (var eventData in PrepareEventsCollection(events))
		{
			eventSpreadSheetLines.Add(SingleEventResponseToSpreadSheetLine(eventData, calendarsMapping));
		}

		CheckEventIdsForDuplicates(eventSpreadSheetLines);

		return eventSpreadSheetLines;
	}

	private void CheckEventIdsForDuplicates(List<EventSpreadSheetLine> eventSpreadSheetLines)
	{
		if (eventSpreadSheetLines.Count == 0)
		{
			return;
		}

		var duplicateIds = eventSpreadSheetLines
			.Where(e => !string.IsNullOrEmpty(e.Id))
			.GroupBy(e => e.Id)
			.Where(g => g.Count() > 1)
			.Select(g => g.Key)
			.ToHashSet();

		foreach (var eventSpreadSheetLine in eventSpreadSheetLines)
		{
			eventSpreadSheetLine.IsDuplicateId = duplicateIds.Contains(eventSpreadSheetLine.Id);
			if (!string.IsNullOrWhiteSpace(_excelReportSpreadSheetConfig.ReportDuplicateEventIdHighlightColorHtml) && duplicateIds.Contains(eventSpreadSheetLine.Id))
			{
				eventSpreadSheetLine.LineHighLightColor = _excelReportSpreadSheetConfig.ReportDuplicateEventIdHighlightColorHtml;
			}
		}
	}

	private List<Event> PrepareEventsCollection(List<Event> events)
	{
		// Take only events with signups
		// Order them by StartDate then by id  !! NOT only by id
		return events
			.Where(q => q.SignupCount > 0)
			.OrderBy(o => o.StartDate)
			.ThenBy(o => o.Id)
			.ToList();
	}

	private EventSpreadSheetLine SingleEventResponseToSpreadSheetLine(Event eventData, List<Subcalendar> calendarsMapping)
	{
		return new EventSpreadSheetLine
		{
			Id = GetEventId(eventData, calendarsMapping),
			Title = eventData.Title,
			Location = eventData.Location,
			Notes = GetNotes(eventData),
			StartDate = eventData.StartDate.ToString(STRING_DATE_TIME_FORMAT),
			EndDate = eventData.EndDate.ToString(STRING_DATE_TIME_FORMAT),
			CreationDate = eventData.CreationDate.ToString(STRING_DATE_TIME_FORMAT),
			UpdateDate = eventData.UpdateDate?.ToString(STRING_DATE_TIME_FORMAT) ?? default!,
			DeleteDate = eventData.DeleteDate?.ToString(STRING_DATE_TIME_FORMAT) ?? default!,
			SignupVisibility = eventData.SignupVisibility,
			SignupCount = eventData.SignupCount.ToString(),
			Signups = GetSignups(eventData),

			Client2 = eventData.Custom?.Client2,
			Division = GetDivision(eventData, calendarsMapping),
			Medical = eventData.Custom?.MeDicalMedical,
			Priority = GetConcatenatedListValues(eventData.Custom?.PrioritePriority2),
			Category = GetConcatenatedListValues(eventData.Custom?.CateGorieCategory),
			ResponsibleInCharge = eventData.Custom?.ResponsableInCharge,
			ProvincialContract = GetConcatenatedListValues(eventData.Custom?.ContratProvincialContract),
			NbMembersNeeded = eventData.Custom?.NombreDeMembresNecessaires,

			PresencesConcat = GetPresencesConcat(eventData),

			LineHighLightColor = GetLineHighLightColor(eventData, calendarsMapping)
		};
	}

	private string GetNotes(Event eventData)
	{
		if (string.IsNullOrWhiteSpace(eventData.Notes))
		{
			return "";
		}

		// Load the HTML content into an HtmlDocument object
		var document = new HtmlDocument();
		document.LoadHtml(eventData.Notes);

		// Remove HTML entities (tags)
		return document.DocumentNode.InnerText.Trim();
	}

	private string GetLineHighLightColor(Event eventData, List<Subcalendar> calendarsMapping)
	{
		if (string.IsNullOrWhiteSpace(_excelReportSpreadSheetConfig.ReportAttentionRequiredHighlightingColorHtml) || _highlightTitlePattern is null)
		{
			return default!;
		}

		if (_highlightTitlePattern.IsMatch(eventData.Title))
		{
			return _excelReportSpreadSheetConfig.ReportAttentionRequiredHighlightingColorHtml;
		}
		else
		{
			return default!;
		}
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

	private string GetEventId(Event eventData, List<Subcalendar> calendarsMapping)
	{
		var eventDate = eventData.StartDate.ToString(EVENT_ID_EVENT_DATE_FORMAT);
		var eventTime = eventData.StartDate.ToString(EVENT_ID_EVENT_TIME_FORMAT);

		return $"{GetEventIdPrefix(eventData, calendarsMapping)}-{eventDate}-{eventTime}-{GetDivision(eventData, calendarsMapping)}";
	}

	private string GetDivision(Event eventData, List<Subcalendar> calendarsMapping)
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

	private string GetEventIdPrefix(Event eventData, List<Subcalendar> calendarsMapping)
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

	private List<string> GetSubCalendarsOfEvent(Event eventData, List<Subcalendar> calendarsMapping)
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
