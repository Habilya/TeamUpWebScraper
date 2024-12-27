using ErrorOr;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebScraperLibrary.TeamUpAPI;

public interface ITeamUpAPIService
{
	Task<ErrorOr<IEnumerable<Event>>> GetEventsAsync(DateTime dateFrom, DateTime dateTo);
}
