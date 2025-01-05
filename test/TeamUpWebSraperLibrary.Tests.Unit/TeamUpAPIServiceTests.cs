using NSubstitute;
using System.Net;
using System.Text.Json;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class TeamUpAPIServiceTests
{
	const string INPUT_FOLDER = "TeamUpApiServiceFilesInput";

	private readonly VerifySettings _verifySettings;

	public TeamUpAPIServiceTests()
	{
		_verifySettings = new VerifySettings();
		_verifySettings.UseDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TeamUpApiServiceFilesVerrified"));
	}

	[Fact]
	public async Task GetEventsAsync_OkButTextResponse_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsString = "Hello, world!";
		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		logger.Received(1).LogError(
			Arg.Is<JsonException>(ex => ex.Message.Equals("'H' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.")),
			"Unhandled Exception while processing API response"
		);

		logger.Received(1).LogWarning("Response as text:\nHello, world!");

		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	// If an exception assertion is needed
	/*
		// Act
		Func<Task> act = () => _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		await act.Should().ThrowAsync<JsonException>()
			.WithMessage("'H' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.");

		// Note: these assertions are called after the await of async function
	 
	 */


	[Fact]
	public async Task GetEventsAsync_200ShortEventsJsonResult_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, INPUT_FOLDER, "GetEventsAsync_200ShortEventsJsonResult.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		logger.Received(0).LogError(Arg.Any<Exception>(), Arg.Any<string>());
		logger.Received(0).LogWarning(Arg.Any<string>());
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	[Fact]
	public async Task GetEventsAsync_200EmptyJsonResult_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, INPUT_FOLDER, "GetEventsAsync_200EmptyJsonResult.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		logger.Received(0).LogError(Arg.Any<Exception>(), Arg.Any<string>());
		logger.Received(0).LogWarning(Arg.Any<string>());
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	[Fact]
	public async Task GetEventsAsync_400InvalidTokenJsonResult_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, INPUT_FOLDER, "GetEventsAsync_400InvalidTokenJsonResult.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		logger.Received(0).LogError(Arg.Any<Exception>(), Arg.Any<string>());
		logger.Received(0).LogWarning(Arg.Any<string>());
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	[Fact]
	public async Task GetEventsAsync_404CalendarNotFoundJsonResult_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, INPUT_FOLDER, "GetEventsAsync_404CalendarNotFoundJsonResult.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		logger.Received(0).LogError(Arg.Any<Exception>(), Arg.Any<string>());
		logger.Received(0).LogWarning(Arg.Any<string>());
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	[Fact]
	public async Task GetEventsAsync_404RouteNotExistsJsonResult_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, INPUT_FOLDER, "GetEventsAsync_404RouteNotExistsJsonResult.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		logger.Received(0).LogError(Arg.Any<Exception>(), Arg.Any<string>());
		logger.Received(0).LogWarning(Arg.Any<string>());
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	[Fact]
	public async Task GetSubcalendarsAsync_200OkJsonResult_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, INPUT_FOLDER, "GetSubcalendarsAsync_200OkJsonResult.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetSubcalendarsAsync();


		// Assert
		logger.Received(0).LogError(Arg.Any<Exception>(), Arg.Any<string>());
		logger.Received(0).LogWarning(Arg.Any<string>());
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	[Fact]
	public async Task GetSubcalendarsAsync_403NoPermissionJsonResult_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, INPUT_FOLDER, "GetSubcalendarsAsync_403NoPermissionJsonResult.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetSubcalendarsAsync();


		// Assert
		logger.Received(0).LogError(Arg.Any<Exception>(), Arg.Any<string>());
		logger.Received(0).LogWarning(Arg.Any<string>());
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}
}
