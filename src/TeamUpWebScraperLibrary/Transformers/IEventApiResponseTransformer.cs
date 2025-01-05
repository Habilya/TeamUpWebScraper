using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebScraperLibrary.Transformers;

public interface IEventApiResponseTransformer
{
	List<EventSpreadSheetLine> EventApiResponseToSpreadSheetLines(List<Event> events, List<Subcalendar> calendarsMapping);
}
