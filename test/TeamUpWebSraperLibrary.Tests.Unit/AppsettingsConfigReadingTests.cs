using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class AppsettingsConfigReadingTests
{
	private readonly VerifySettings _verifySettings;

	public AppsettingsConfigReadingTests()
	{
		_verifySettings = new VerifySettings();
		_verifySettings.UseDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"AppsettingsConfigReadingTestFiles"));
	}


	[Fact]
	public async Task ReadTeamUpAPIConfigSection_ShouldMatchExpected_WhenConfigValid()
	{
		// Arrange


		// Act
		var actual = TestsHelper.ReadConfigIntoModel<TeamUpApiConfiguration>(@"AppsettingsConfigReadingTestFiles\ValidTestsConfig.json", AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API);


		// Assert
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}


	[Fact]
	public async Task ReadExcelReportSpreadSheetConfigSection_ShouldMatchExpected_WhenConfigValid()
	{
		// Arrange


		// Act
		var actual = TestsHelper.ReadConfigIntoModel<ExcelReportSpreadSheetConfig>(@"AppsettingsConfigReadingTestFiles\ValidTestsConfig.json", AppsettingsConstants.CONFIG_SECTION_NAME_EXCEL_SPREADSHEET);


		// Assert
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}
}
