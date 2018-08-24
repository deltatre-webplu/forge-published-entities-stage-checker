using ForgePublishedEntitiesStageChecker.Contracts;
using ForgePublishedEntitiesStageChecker.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ForgePublishedEntitiesStageChecker.Report
{
	public class ReportCreator
	{
		public void CreateJsonReport(string reportFilePath, IEnumerable<Entity> publishedEntitiesWithUnexpectedStage)
		{
			FileSystemHelpers.EnsureFileDirectoryExists(reportFilePath);

			using (var streamWriter = File.CreateText(reportFilePath))
			{
				var serializer = new JsonSerializer
				{
					Formatting = Formatting.Indented
				};
				serializer.Serialize(streamWriter, publishedEntitiesWithUnexpectedStage);
			}
		}
	}
}
