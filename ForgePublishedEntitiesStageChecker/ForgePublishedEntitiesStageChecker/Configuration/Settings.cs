namespace ForgePublishedEntitiesStageChecker.Configuration
{
	public class Settings
	{
		public string MongoConnString { get; }
		public string LogFilePath { get; }
		public string ReportFilePath { get; }

		public Settings(string mongoConnString, string logFilePath, string reportFilePath)
		{
			this.MongoConnString = mongoConnString;
			this.LogFilePath = logFilePath;
			this.ReportFilePath = reportFilePath;
		}
	}
}
