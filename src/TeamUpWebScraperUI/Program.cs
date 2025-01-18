using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TeamUpWebScraperLibrary;

namespace TeamUpWebScraperUI;

public static class Program
{
	const string CONFIG_JSON_FILE_PATH = @"config\appsettings.json";

	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main()
	{
		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		if (!CheckForConfig())
		{
			return;
		}

		var host = CreateHostBuilder().Build();
		ServiceProvider = host.Services;

		Application.Run(ServiceProvider.GetRequiredService<Dashboard>());
	}

	private static bool CheckForConfig()
	{
		var configFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIG_JSON_FILE_PATH);
		if (!File.Exists(configFullPath))
		{
			MessageBox.Show($"Make sure you have the config file {CONFIG_JSON_FILE_PATH}, it is not deployed by default.", "appsettings.json not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return false;
		}

		return true;
	}

	public static IServiceProvider ServiceProvider { get; private set; } = default!;

	static IHostBuilder CreateHostBuilder()
	{
		return Host
			.CreateDefaultBuilder()
			.ConfigureAppConfiguration((hostContext, config) =>
			{
				config.AddJsonFile(CONFIG_JSON_FILE_PATH, optional: false)
					.AddEnvironmentVariables()
					.Build();
			})
			.ConfigureLogging((hostContext, logging) =>
			{
				logging.AddTeamUpWebScraperLogging(hostContext.Configuration);
			})
			.ConfigureServices((context, services) =>
			{
				services.AddTransient<Dashboard>();
				services.AddSingleton<TeamUpController>();
				services.AddTeamUpWebScraperLibraryServices(context);
			});
	}
}
