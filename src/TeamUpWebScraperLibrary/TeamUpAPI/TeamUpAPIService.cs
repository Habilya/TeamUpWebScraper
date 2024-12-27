using ErrorOr;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebScraperLibrary.TeamUpAPI;

public class TeamUpAPIService : ITeamUpAPIService
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly HttpClient _httpClient;
	private readonly TeamUpApiConfiguration _teamUpApiConfiguration;
	private readonly ILoggerAdapter<TeamUpAPIService> _logger;

	public TeamUpAPIService(
		IHttpClientFactory httpClientFactory,
		TeamUpApiConfiguration teamUpApiConfiguration,
		ILoggerAdapter<TeamUpAPIService> logger)
	{
		_logger = logger;
		_teamUpApiConfiguration = teamUpApiConfiguration;
		_httpClientFactory = httpClientFactory;
		_httpClient = _httpClientFactory.CreateClient(TeamUpApiConstants.HTTP_CLIENTNAME);
	}

	public async Task<ErrorOr<IEnumerable<Event>>> GetEventsAsync(DateTime dateFrom, DateTime dateTo)
	{
		string route = "/events";
		string queryStringParams = $"?startDate={DateTimeToString(dateFrom)}&endDate={DateTimeToString(dateTo)}&tz={_teamUpApiConfiguration.TimeZone}";

		var response = await _httpClient.GetAsync($"/{_teamUpApiConfiguration.CalendarId}{route}{queryStringParams}");

		if (response.StatusCode.Equals(HttpStatusCode.BadRequest))
		{
			try
			{
				var responseErrorAsString = await response.Content.ReadAsStringAsync();
				if (responseErrorAsString.Equals(TeamUpApiConstants.API_BADREQUEST_KEYMISSING))
				{
					return Errors.Events.ApiTokenMissingOrInvalid();
				}
				else
				{
					var errorBody = await response.Content.ReadFromJsonAsync<ErrorResponse>();
					return Errors.Events.ApiRequestError(errorBody!.error);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled Exception while processing bad request");
				return Errors.Events.BadRequest();
			}
		}

		if (response.StatusCode.Equals(HttpStatusCode.NotFound))
		{
			return Errors.Events.NotFound();
		}


		var responseBody = await response.Content.ReadFromJsonAsync<List<Event>>();

		return responseBody is not null
			? responseBody
			: Errors.Events.NotFound();
	}

	private string DateTimeToString(DateTime dateFrom)
	{
		return dateFrom.ToString(TeamUpApiConstants.API_ROUTE_DATE_FORMAT, CultureInfo.InvariantCulture);
	}
}
