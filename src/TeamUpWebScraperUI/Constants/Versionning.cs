using System.Reflection;
using System.Text.RegularExpressions;

namespace TeamUpWebScraperUI.Constants;

public static class Versionning
{
	public const string ENV_BUILD_NUMBER = "BUILD_NUMBER";
	public const string ENV_DOTNET_ENVIRONMENT = "DOTNET_ENVIRONMENT";
	public const string ENV_DEFAULT_VARIABLE_ANSWER = "N/A";


	#region I couldn't get ENV Variables to work...
	private static string GetBuildNumber()
	{
		return Environment.GetEnvironmentVariable(ENV_BUILD_NUMBER) ?? ENV_DEFAULT_VARIABLE_ANSWER;
	}

	private static string GetRunningEnvironement()
	{
		return Environment.GetEnvironmentVariable(ENV_DOTNET_ENVIRONMENT) ?? ENV_DEFAULT_VARIABLE_ANSWER;
	}
	#endregion

	public static string GetVersion()
	{
		try
		{
			var roughVersion = typeof(Program).Assembly.GetName().Version?.ToString();
			var version = Regex
				.Match(roughVersion!, @"^([0-9]*.[0-9]*.[0-9]*)(?=.[0-9]*)")
				.ToString();
			return version;
		}
		catch (Exception)
		{
			return ENV_DEFAULT_VARIABLE_ANSWER;
		}
	}

	private static string GetVersionPostfix()
	{
		try
		{
			return Assembly.GetEntryAssembly()!
				.GetCustomAttribute<AssemblyFileVersionAttribute>()!
				.Version;
		}
		catch (Exception)
		{
			return ENV_DEFAULT_VARIABLE_ANSWER;
		}
	}

	public static string GetVersionOneLiner()
	{
		return $"v{GetVersion()}-{GetBuildNumber()} {GetRunningEnvironement()}";
	}
}
