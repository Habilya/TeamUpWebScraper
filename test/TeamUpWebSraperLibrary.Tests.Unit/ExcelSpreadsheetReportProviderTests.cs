using NSubstitute;
using System.Net;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.Providers;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.Transformers;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class ExcelSpreadsheetReportProviderTests
{
	private readonly VerifySettings _verifySettings;

	public ExcelSpreadsheetReportProviderTests()
	{
		_verifySettings = new VerifySettings();
		_verifySettings.UseDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ExcelSpreadsheetReportProviderTestFiles"));
	}


	[Fact]
	public async Task EventApiResponseToExcelReportFile_ShouldVerrify()
	{
		// Arrange
		#region Arrangement of APIService response
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var excelReportSpreadSheetConfig = Substitute.For<ExcelReportSpreadSheetConfig>();
		var loggerApiService = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();
		var loggerExcelSpreadSheet = Substitute.For<ILoggerAdapter<ExcelSpreadsheetReportProvider>>();
		var dateTimeProvider = Substitute.For<IDateTimeProvider>();
		// For now no substitution
		var xlWorkBookFactory = new XLWorkBookFactory();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		excelReportSpreadSheetConfig.ReportSpreadSheetName = "Export";
		excelReportSpreadSheetConfig.ReportHeaderLine = 1;
		excelReportSpreadSheetConfig.ReportSignupsLimit = 60;
		excelReportSpreadSheetConfig.ReportHeaderBackgroundColorHtml = "#87cefa";
		excelReportSpreadSheetConfig.ReportAttentionRequiredHighlightingColorHtml = "#FFFF00";
		excelReportSpreadSheetConfig.ReportDuplicateEventIdHighlightColorHtml = "#FFA500";
		excelReportSpreadSheetConfig.EventTitlesToHighLightPattern = "Cancel|Annul";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseToReportModelAcceptanceTests\SeptemberEventsAPIResponse.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var teamUpAPIService = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, loggerApiService);
		// Doesn't really matter as the response is mocked
		var dateFromParam = new DateTime(2024, 11, 1);
		var dateToParam = new DateTime(2024, 11, 30);

		dateTimeProvider.DateTimeNow.Returns(new DateTime(2022, 2, 2, 20, 0, 0));
		var excelSaveFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EventApiResponseToExcelReportFile.xlsx");
		var excelExpectedFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ExcelSpreadsheetReportProviderTestFiles\EventApiResponseToExcelReportFile.xlsx");
		#endregion

		var apiResponseTransformer = new EventApiResponseTransformer(excelReportSpreadSheetConfig);
		var subCalendars = TestsHelper.ReadSubCalendarsFromJSON();
		var _sut = new ExcelSpreadsheetReportProvider(loggerExcelSpreadSheet, dateTimeProvider, xlWorkBookFactory, excelReportSpreadSheetConfig);

		// Act
		var apiResponse = await teamUpAPIService.GetEventsAsync(dateFromParam, dateToParam);
		var transformedSheetLines = apiResponseTransformer.EventApiResponseToSpreadSheetLines(apiResponse.Value.Events, subCalendars);

		// Uncomment if some Exploration testing is needed
		//_sut.SaveExcelReport(excelSaveFullPath, transformedSheetLines);

		// Assert
		await Verify(transformedSheetLines, _verifySettings).DontScrubDateTimes();
	}
}
