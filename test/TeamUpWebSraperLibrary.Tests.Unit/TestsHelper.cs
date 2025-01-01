using Microsoft.Extensions.Configuration;
using NSubstitute;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public static class TestsHelper
{
	public static TeamUpApiConfiguration ReadConfigIntoModel(string configRelativePath)
	{
		var builder = new ConfigurationBuilder();
		builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			   .AddJsonFile(configRelativePath, optional: false, reloadOnChange: true);

		var configuration = builder.Build();  // Build the configuration

		// Create the TeamUpApiConfiguration object to bind the section to
		var teamUpApiConfiguration = new TeamUpApiConfiguration();

		// Bind the configuration section to the model
		configuration.GetSection(TeamUpApiConstants.CONFIG_SECTION_NAME).Bind(teamUpApiConfiguration);

		return teamUpApiConfiguration;
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
}
