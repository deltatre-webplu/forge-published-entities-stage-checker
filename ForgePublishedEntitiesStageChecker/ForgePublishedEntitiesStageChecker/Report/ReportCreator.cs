using ForgePublishedEntitiesStageChecker.Contracts;
using ForgePublishedEntitiesStageChecker.Helpers;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace ForgePublishedEntitiesStageChecker.Report
{
	public class ReportCreator
	{
		public void CreateJsonReport(string reportFilePath, IEnumerable<Entity> publishedEntitiesWithUnexpectedStage)
		{
			Log.Information("Creating JSON report...");

			FileSystemHelpers.EnsureFileDirectoryExists(reportFilePath);

			using (var streamWriter = File.CreateText(reportFilePath))
			{
				var serializer = new JsonSerializer
				{
					Formatting = Formatting.Indented
				};
				serializer.Serialize(streamWriter, publishedEntitiesWithUnexpectedStage);
			}

			Log.Debug("JSON report successfully created");
		}

		private static string GetReportFileDirectoryPath(string reportFileDirectoryPath)
		{
			throw new NotImplementedException();
		}
	}
}
