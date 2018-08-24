using ForgePublishedEntitiesStageChecker.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ForgePublishedEntitiesStageChecker.Report
{
	public class ReportCreator
	{
		public void CreateJsonReport(string reportFilePath, IEnumerable<Entity> publishedEntitiesWithUnexpectedStage)
		{
			EnsureDirectoryPathExists(reportFilePath);

			using (var streamWriter = File.CreateText(reportFilePath))
			{
				var serializer = new JsonSerializer
				{
					Formatting = Formatting.Indented
				};
				serializer.Serialize(streamWriter, publishedEntitiesWithUnexpectedStage);
			}
		}

		private static void EnsureDirectoryPathExists(string reportFilePath)
		{
			var reportDirectoryPath = Path.GetDirectoryName(reportFilePath);
			Directory.CreateDirectory(reportDirectoryPath);
		}
	}
}
