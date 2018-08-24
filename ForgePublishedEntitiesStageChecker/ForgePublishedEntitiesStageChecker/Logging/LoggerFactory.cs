using Serilog;

namespace ForgePublishedEntitiesStageChecker.Logging
{
	public class LoggerFactory
	{
		public ILogger CreateLogger()
		{
			var logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.CreateLogger();

			return logger;
		}
	}
}
