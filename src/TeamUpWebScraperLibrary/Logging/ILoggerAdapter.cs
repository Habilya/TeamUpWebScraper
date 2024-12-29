
namespace TeamUpWebScraperLibrary.Logging
{
	public interface ILoggerAdapter<TType>
	{
		void LogError(Exception? exception, string? message, params object?[] args);
		void LogInformation(string? message, params object?[] args);
		void LogWarning(string? message, params object?[] args);
	}
}
