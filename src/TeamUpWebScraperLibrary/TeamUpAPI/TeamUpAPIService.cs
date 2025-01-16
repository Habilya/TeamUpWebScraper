using ErrorOr;
using Polly;
using Polly.Retry;
using System.Diagnostics;
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

	private readonly AsyncRetryPolicy _asyncRetryPolicy;

	public TeamUpAPIService(
		IHttpClientFactory httpClientFactory,
		TeamUpApiConfiguration teamUpApiConfiguration,
		ILoggerAdapter<TeamUpAPIService> logger)
	{
		_logger = logger;
		_teamUpApiConfiguration = teamUpApiConfiguration;
		_httpClientFactory = httpClientFactory;
		_httpClient = _httpClientFactory.CreateClient(TeamUpApiConstants.HTTP_CLIENTNAME);

		// Handle network-related errors, Retry up to 3 times, Exponential backoff: 1, 2, 4 seconds
		_asyncRetryPolicy = Policy
			.Handle<HttpRequestException>()
			.WaitAndRetryAsync(
				retryCount: _teamUpApiConfiguration.MaxHttpCallRetries,
				sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
				onRetry: (exception, timespan, attempt, context) =>
				{
					_logger.LogWarning($"(Retry {attempt}) after {timespan.TotalSeconds} seconds, due to: {exception?.GetType()} {exception?.Message}");
				});
	}

	public async Task<ErrorOr<EventResponse>> GetEventsAsync(DateTime dateFrom, DateTime dateTo)
	{
		string route = "events";
		string queryStringParams = $"?startDate={DateTimeToString(dateFrom)}&endDate={DateTimeToString(dateTo)}&tz={_teamUpApiConfiguration.TimeZone}";

		var response = await _asyncRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{route}{queryStringParams}"));

		return await ProcessApiResponse<EventResponse>(response);
	}

	public async Task<ErrorOr<SubcalendarResponse>> GetSubcalendarsAsync()
	{
		var response = await _asyncRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"subcalendars"));

		return await ProcessApiResponse<SubcalendarResponse>(response);
	}

	private async Task<ErrorOr<T>> ProcessApiResponse<T>(HttpResponseMessage response)
	{
		try
		{
			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
					var responseBody = await response.Content.ReadFromJsonAsync<T>();

					if (responseBody is null)
					{
						return Errors.TeamUpAPIServiceErrors.ApiResponseWithTextError("Parsed response is null.");
					}

					return responseBody;
				default:
					var errorBody = await response.Content.ReadFromJsonAsync<ErrorResponse>();
					return Errors.TeamUpAPIServiceErrors.ApiResponseWithJsonError(errorBody!.Error);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Demystify(), "Unhandled Exception while processing API response");
			await LogResponseAsText(response);
			return Errors.TeamUpAPIServiceErrors.ApiResponseWithTextError("Unhandled Exception was thrown while parsing response, more details in the log file.");
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
			_logger.LogError(ex.Demystify(), "Unhandled Exception while LogResponseAsText");
		}
	}

	private string DateTimeToString(DateTime dateTime)
	{
		return dateTime.ToString(TeamUpApiConstants.API_ROUTE_DATE_FORMAT, CultureInfo.InvariantCulture);
	}
}
