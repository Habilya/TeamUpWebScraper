using NSubstitute;
using Shouldly;
using System.Net;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.Transformers;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class EventApiResponseToReportModelAcceptanceTests
{
	private readonly VerifySettings _verifySettings;

	public EventApiResponseToReportModelAcceptanceTests()
	{
		_verifySettings = new VerifySettings();
		_verifySettings.UseDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseToReportModelAcceptanceTests"));
	}

	[Fact]
	public async Task EventApiResponseToSpreadSheetLines_ShouldMatchExpected()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var excelReportSpreadSheetConfig = Substitute.For<ExcelReportSpreadSheetConfig>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseToReportModelAcceptanceTests\SeptemberEventsAPIResponse.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var teamUpAPIService = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);
		// Doesn't really matter as the response is mocked
		var dateFromParam = new DateTime(2024, 11, 1);
		var dateToParam = new DateTime(2024, 11, 30);

		var _sut = new EventApiResponseTransformer(excelReportSpreadSheetConfig);
		var subCalendars = TestsHelper.ReadSubCalendarsFromJSON();

		// Act
		var apiResponse = await teamUpAPIService.GetEventsAsync(dateFromParam, dateToParam);
		var actual = _sut.EventApiResponseToSpreadSheetLines(apiResponse.Value.Events, subCalendars);


		// Assert
		actual.ShouldNotBeNull();
		actual.Count.ShouldBe(82);
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	[Fact]
	public async Task EventApiResponseToSpreadSheetLines_ShouldVerrify()
	{
		// Arrange
		#region Arrangement of APIService response
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var excelReportSpreadSheetConfig = Substitute.For<ExcelReportSpreadSheetConfig>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseToReportModelAcceptanceTests\SeptemberEventsAPIResponse.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var teamUpAPIService = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);
		// Doesn't really matter as the response is mocked
		var dateFromParam = new DateTime(2024, 11, 1);
		var dateToParam = new DateTime(2024, 11, 30);
		#endregion

		var _sut = new EventApiResponseTransformer(excelReportSpreadSheetConfig);
		var subCalendars = TestsHelper.ReadSubCalendarsFromJSON();

		// Act
		var apiResponse = await teamUpAPIService.GetEventsAsync(dateFromParam, dateToParam);
		var actual = _sut.EventApiResponseToSpreadSheetLines(apiResponse.Value.Events, subCalendars);


		// Assert
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}
}
