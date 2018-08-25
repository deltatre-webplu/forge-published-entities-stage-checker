namespace ForgePublishedEntitiesStageChecker.Configuration
{
	public class Settings
	{
		public string ConfigFilePath { get; }
		public string ReportDirectoryPath { get; }

		public Settings(string configFilePath, string reportDirectoryPath)
		{
			this.ConfigFilePath = configFilePath;
			this.ReportDirectoryPath = reportDirectoryPath;
		}
	}
}
