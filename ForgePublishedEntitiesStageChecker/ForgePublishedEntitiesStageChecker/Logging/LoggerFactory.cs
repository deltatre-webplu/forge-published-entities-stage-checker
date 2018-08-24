using Microsoft.Extensions.Configuration;
using Serilog;

namespace ForgePublishedEntitiesStageChecker.Logging
{
	public class LoggerFactory
	{
		public ILogger CreateLogger(IConfiguration configuration)
		{
			var logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			return logger;
		}
	}
}
