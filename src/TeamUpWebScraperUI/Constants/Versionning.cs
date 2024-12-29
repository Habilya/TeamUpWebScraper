namespace TeamUpWebScraperUI.Constants;

public static class Versionning
{
	public const string ENV_BUILD_NUMBER = "BUILD_NUMBER";
	public const string ENV_DOTNET_ENVIRONMENT = "DOTNET_ENVIRONMENT";
	public const string ENV_DEFAULT_VARIABLE_ANSWER = "N/A";

	public static string GetVersion()
	{
		return typeof(Program).Assembly.GetName().Version?.ToString() ?? ENV_DEFAULT_VARIABLE_ANSWER;
	}

	public static string GetBuildNumber()
	{
		return Environment.GetEnvironmentVariable(ENV_BUILD_NUMBER) ?? ENV_DEFAULT_VARIABLE_ANSWER;
	}

	public static string GetRunningEnvironement()
	{
		return Environment.GetEnvironmentVariable(ENV_DOTNET_ENVIRONMENT) ?? ENV_DEFAULT_VARIABLE_ANSWER;
	}

	public static string GetVersionOneLiner()
	{
		return $"v{GetVersion()}-{GetBuildNumber()} env: {GetRunningEnvironement()}";
	}
}
