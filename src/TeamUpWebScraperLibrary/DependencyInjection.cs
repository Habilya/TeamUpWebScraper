using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TeamUpWebScraperLibrary.DisplayGridView;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.Providers;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.Transformers;
using TeamUpWebScraperLibrary.Validators;

namespace TeamUpWebScraperLibrary;

public static class DependencyInjection
{
	public static ILoggingBuilder AddTeamUpWebScraperLogging(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
	{
		var serilogLogger = new LoggerConfiguration()
			.ReadFrom.Configuration(configuration)
			.Enrich.FromLogContext()
			.CreateLogger();

		// Clear default logging providers and add Serilog
		loggingBuilder.ClearProviders();
		loggingBuilder.AddSerilog(serilogLogger);

		return loggingBuilder;
	}

	public static IServiceCollection AddTeamUpWebScraperLibraryServices(this IServiceCollection services, HostBuilderContext context)
	{
		// Register the services from the library
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
		services.AddSingleton<InputValidation>();
		services.AddSingleton<ITeamUpAPIService, TeamUpAPIService>();
		services.AddSingleton<IXLWorkBookFactory, XLWorkBookFactory>();
		services.AddSingleton<IEventApiResponseTransformer, EventApiResponseTransformer>();
		services.AddSingleton<IExcelSpreadsheetReportProvider, ExcelSpreadsheetReportProvider>();
		services.AddSingleton<IDisplayGridViewProvider, DisplayGridViewProvider>();

		// Example of adding HttpClient if needed for the library
		services.AddHttpClient(TeamUpApiConstants.HTTP_CLIENTNAME, (serviceProvider, httpClient) =>
		{
			var config = serviceProvider.GetRequiredService<IConfiguration>();
			var baseURL = config.GetValue<string>($"{AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API}:{TeamUpApiConstants.CONFIG_BaseURL_NAME}");
			var calendarId = config.GetValue<string>($"{AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API}:{TeamUpApiConstants.CONFIG_CalendarId_NAME}");
			var teamupToken = config.GetValue<string>($"{AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API}:{TeamUpApiConstants.CONFIG_TeamupToken_NAME}");

			httpClient.BaseAddress = new Uri(baseURL + calendarId + "/");
			httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
			httpClient.DefaultRequestHeaders.Add(TeamUpApiConstants.API_TOKEN_HEADER_NAME, teamupToken);
		});

		// Read appsettings.json into appropriate models

		// !!! BUG or FEATURE? !!! This Bind is uncapable of reading model annotations
		// ex: [JsonPropertyName("name")]
		// Probably because config is supposed to be concieved by the developer
		// and everythig is supposed to be rightly named
		#region TeamUpApiConfiguration as a dependency
		var teamUpApiConfiguration = new TeamUpApiConfiguration();
		context.Configuration.GetSection(AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API).Bind(teamUpApiConfiguration);
		services.AddSingleton(teamUpApiConfiguration);
		#endregion

		#region ExcelReportSpreadSheetConfiguration as a dependency
		var excelReportSpreadSheetConfiguration = new ExcelReportSpreadSheetConfig();
		context.Configuration.GetSection(AppsettingsConstants.CONFIG_SECTION_NAME_EXCEL_SPREADSHEET).Bind(excelReportSpreadSheetConfiguration);
		services.AddSingleton(excelReportSpreadSheetConfiguration);
		#endregion

		// Add other necessary services as needed
		return services;
	}
}
