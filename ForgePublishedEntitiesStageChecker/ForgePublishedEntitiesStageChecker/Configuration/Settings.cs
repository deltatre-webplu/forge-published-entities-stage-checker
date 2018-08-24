namespace ForgePublishedEntitiesStageChecker.Configuration
{
	public class Settings
	{
		public string MongoConnString { get; }
		public string ReportFilePath { get; }

		public Settings(string mongoConnString, string reportFilePath)
		{
			this.MongoConnString = mongoConnString;
			this.ReportFilePath = reportFilePath;
		}
	}
}
