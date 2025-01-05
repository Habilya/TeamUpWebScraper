using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class CustomHttpMessageHandler : HttpMessageHandler
{
	private readonly HttpResponseMessage _response;

	public CustomHttpMessageHandler(HttpResponseMessage response)
	{
		_response = response;
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_response);
	}
}

public static class TestsHelper
{
	public static T? ReadConfigIntoModel<T>(string configRelativePath, string configSectionName)
	{
		var builder = new ConfigurationBuilder();
		builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			   .AddJsonFile(configRelativePath, optional: false, reloadOnChange: true);

		var configuration = builder.Build();  // Build the configuration

		var section = configuration.GetSection(configSectionName);
		T? configModel = section.Get<T>();

		return configModel;
	}

	public static void ArrangeHttpClientMock(IHttpClientFactory iHttpClientFactory, HttpResponseMessage httpResponseMessage)
	{
		var handler = new CustomHttpMessageHandler(httpResponseMessage);

		// Create an instance of HttpClient using the mock handler
		var httpClient = new HttpClient(handler)
		{
			// Base address can be any valid URI
			BaseAddress = new Uri("http://localhost/")
		};

		iHttpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);
	}

	public static DateTime? ParseDateForTest(string? dateAsString, string DateInputFormat)
	{
		if (string.IsNullOrWhiteSpace(dateAsString))
		{
			return null;
		}

		DateTime.TryParseExact(dateAsString, DateInputFormat, null, System.Globalization.DateTimeStyles.None, out DateTime dateResult);
		return dateResult;
	}

	public static List<Subcalendar> ReadSubCalendarsFromJSON(string? subCalendarsJSONFullPath = null)
	{
		if (string.IsNullOrWhiteSpace(subCalendarsJSONFullPath))
		{
			subCalendarsJSONFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseTransformerTestFiles\GetSubcalendarsAsync_CleanJsonResult.json");
		}

		string json = File.ReadAllText(subCalendarsJSONFullPath);
		return JsonConvert.DeserializeObject<List<Subcalendar>>(json)!;
	}
}
