﻿using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TeamUpWebScraperLibrary.Logging;

public class LoggerAdapter<TType> : ILoggerAdapter<TType>
{
	private readonly ILogger<TType> _logger;

	public LoggerAdapter(ILogger<TType> logger)
	{
		_logger = logger;
	}

	public void LogInformation(string? message, params object?[] args)
	{
		_logger.LogInformation(message, args);
	}

	public void LogWarning(string? message, params object?[] args)
	{
		_logger.LogWarning(message, args);
	}

	public void LogError(Exception? exception, string? message, params object?[] args)
	{
		_logger.LogError(exception?.Demystify(), message, args);
	}
}
