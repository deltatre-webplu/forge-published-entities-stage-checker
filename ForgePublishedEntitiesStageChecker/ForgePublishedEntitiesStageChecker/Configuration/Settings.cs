namespace ForgePublishedEntitiesStageChecker.Configuration
{
	public class Settings
	{
		public string MongoConnString { get; }
		public string ReportDirectoryPath { get; }

		public Settings(string mongoConnString, string reportDirectoryPath)
		{
			this.MongoConnString = mongoConnString;
			this.ReportDirectoryPath = reportDirectoryPath;
		}
	}
}
