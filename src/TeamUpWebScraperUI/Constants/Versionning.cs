using System.Reflection;
using System.Text.RegularExpressions;

namespace TeamUpWebScraperUI.Constants;

public static class Versionning
{
	public const string ENV_DEFAULT_VARIABLE_ANSWER = "N/A";

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
		return $"v{GetVersion()}-{GetVersionPostfix()}";
	}
}
