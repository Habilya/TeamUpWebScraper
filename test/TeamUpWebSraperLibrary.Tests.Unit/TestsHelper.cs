using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace TeamUpWebSraperLibrary.Tests.Unit;

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
}
