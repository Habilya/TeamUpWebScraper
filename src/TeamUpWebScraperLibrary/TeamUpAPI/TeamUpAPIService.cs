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

	public async Task<ErrorOr<EventResponse>> GetEventsAsync(DateTime dateFrom, DateTime dateTo)
	{
		string route = "events";
		string queryStringParams = $"?startDate={DateTimeToString(dateFrom)}&endDate={DateTimeToString(dateTo)}&tz={_teamUpApiConfiguration.TimeZone}";

		var response = await _httpClient.GetAsync($"{route}{queryStringParams}");

		switch (response.StatusCode)
		{
			case HttpStatusCode.OK:
				return await GetEventsProcessOkResponse(response);
			case HttpStatusCode.BadRequest:
				return await GetEventsProcessBadRequestResponse(response);
			case HttpStatusCode.NotFound:
				return Errors.Events.NotFound();
			default:
				return await GetEventsProcessAnyOtherRequestType(response);
		}
	}

	private async Task<ErrorOr<EventResponse>> GetEventsProcessAnyOtherRequestType(HttpResponseMessage response)
	{
		await LogResponseAsText(response);
		return Errors.Events.BadRequest();
	}

	private async Task<ErrorOr<EventResponse>> GetEventsProcessBadRequestResponse(HttpResponseMessage response)
	{
		string responseErrorAsString = "";
		try
		{
			responseErrorAsString = await response.Content.ReadAsStringAsync();
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
			_logger.LogWarning($"Response as text:\n{responseErrorAsString}");
			_logger.LogError(ex, "Unhandled Exception while processing bad request");
			return Errors.Events.BadRequest();
		}
	}

	private async Task<ErrorOr<EventResponse>> GetEventsProcessOkResponse(HttpResponseMessage response)
	{
		try
		{
			var responseBody = await response.Content.ReadFromJsonAsync<EventResponse>();

			if (responseBody is null)
			{
				return Errors.Events.NotFound();
			}

			return responseBody;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled Exception while GetEventsProcessOkResponse");
			await LogResponseAsText(response);
			return Errors.Events.BadRequest();
		}
	}

	private async Task LogResponseAsText(HttpResponseMessage response)
	{
		try
		{
			var responseErrorAsString = await response.Content.ReadAsStringAsync();
			_logger.LogWarning($"Response as text:\n{responseErrorAsString}");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled Exception while LogResponseAsText");
		}
	}

	private string DateTimeToString(DateTime dateFrom)
	{
		return dateFrom.ToString(TeamUpApiConstants.API_ROUTE_DATE_FORMAT, CultureInfo.InvariantCulture);
	}
}
