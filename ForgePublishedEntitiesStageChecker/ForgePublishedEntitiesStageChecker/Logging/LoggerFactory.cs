using ForgePublishedEntitiesStageChecker.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace ForgePublishedEntitiesStageChecker.Logging
{
	public class LoggerFactory
	{
		public ILogger CreateLogger(string logFilePath)
		{
			FileSystemHelpers.EnsureFileDirectoryExists(logFilePath);

			var logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File(new JsonFormatter(), logFilePath, rollingInterval: RollingInterval.Day)
				.WriteTo.Console(LogEventLevel.Information)
				.CreateLogger();

			return logger;
		}
	}
}
