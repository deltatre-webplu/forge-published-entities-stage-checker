using ForgePublishedEntitiesStageChecker.Helpers;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;

namespace ForgePublishedEntitiesStageChecker.Logging
{
	public class LoggerFactory
	{
		private const string LogFilePathKey = "LogFilePath";
		private const string DefaultLogFilePath = "logs.txt";

		public ILogger CreateLogger(IConfiguration configuration)
		{
			var logFilePath = GetLogFilePath(configuration);
			if (IsCustomLogFilePath(logFilePath))
				FileSystemHelpers.EnsureFileDirectoryExists(logFilePath);

			var logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File(new JsonFormatter(), logFilePath, rollingInterval: RollingInterval.Day)
				.WriteTo.Console()
				.CreateLogger();

			return logger;
		}

		private static string GetLogFilePath(IConfiguration configuration)
		{
			var configuredLogFilePath = configuration[LogFilePathKey];
			return string.IsNullOrWhiteSpace(configuredLogFilePath) ? DefaultLogFilePath : configuredLogFilePath;
		}

		private static bool IsCustomLogFilePath(string logFilePath)
		{
			return logFilePath != DefaultLogFilePath;
		}
	}
}
