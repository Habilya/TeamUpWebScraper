using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
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

namespace TeamUpWebScraperUI;

public static class Program
{
	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main()
	{
		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();

		var host = CreateHostBuilder().Build();
		ServiceProvider = host.Services;

		Application.Run(ServiceProvider.GetRequiredService<Dashboard>());
	}

	public static IServiceProvider ServiceProvider { get; private set; } = default!;

	static IHostBuilder CreateHostBuilder()
	{
		return Host
			.CreateDefaultBuilder()
			.ConfigureAppConfiguration((hostContext, config) =>
			{
				config.AddJsonFile(@"config\appsettings.json", optional: false)
					.AddEnvironmentVariables()
					.Build();
			})
			.ConfigureLogging((hostContext, logging) =>
			{
				var serilogLogger = new LoggerConfiguration()
					.ReadFrom.Configuration(hostContext.Configuration)
					.Enrich.FromLogContext()
					.CreateLogger();

				logging.ClearProviders();
				logging.AddSerilog(serilogLogger);
			})
			.ConfigureServices((context, services) =>
			{
				services.AddTransient<Dashboard>();
				services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
				services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
				services.AddSingleton<InputValidation>();
				services.AddSingleton<ITeamUpAPIService, TeamUpAPIService>();
				services.AddSingleton<IXLWorkBookFactory, XLWorkBookFactory>();
				services.AddSingleton<IEventApiResponseTransformer, EventApiResponseTransformer>();
				services.AddSingleton<IExcelSpreadsheetReportProvider, ExcelSpreadsheetReportProvider>();
				services.AddSingleton<IDisplayGridViewProvider, DisplayGridViewProvider>();
				services.AddHttpClient(TeamUpApiConstants.HTTP_CLIENTNAME, httpClient =>
				{
					var baseURL = context.Configuration.GetValue<string>($"{AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API}:{TeamUpApiConstants.CONFIG_BaseURL_NAME}");
					var calendarId = context.Configuration.GetValue<string>($"{AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API}:{TeamUpApiConstants.CONFIG_CalendarId_NAME}");
					var teamupToken = context.Configuration.GetValue<string>($"{AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API}:{TeamUpApiConstants.CONFIG_TeamupToken_NAME}");

					httpClient.BaseAddress = new Uri(baseURL + calendarId + "/");
					httpClient.DefaultRequestHeaders
						.Add(HeaderNames.Accept, "application/json");
					httpClient.DefaultRequestHeaders
						.Add(TeamUpApiConstants.API_TOKEN_HEADER_NAME, teamupToken);
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
			});
	}
}
