using ErrorOr;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebScraperLibrary.TeamUpAPI;

public interface ITeamUpAPIService
{
	Task<ErrorOr<EventResponse>> GetEventsAsync(DateTime dateFrom, DateTime dateTo);

	Task<ErrorOr<SubcalendarResponse>> GetSubcalendarsAsync();
}
