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
		public void CreateJsonReport(
			string reportDirectoryPath, 
			IEnumerable<Entity> publishedEntitiesWithUnexpectedStage, 
			string databaseName)
		{
			Log.Information("Creating JSON report for database {DatabaseName}...", databaseName);

			var reportFilePath = GetReportFilePath(reportDirectoryPath, databaseName);
			Log.Debug("Computed report file absolute path for database {DatabaseName} is {ReportFilePath}", databaseName, reportFilePath);
			FileSystemHelpers.EnsureFileDirectoryExists(reportFilePath);

			using (var streamWriter = File.CreateText(reportFilePath))
			{
				var serializer = new JsonSerializer
				{
					Formatting = Formatting.Indented
				};
				serializer.Serialize(streamWriter, publishedEntitiesWithUnexpectedStage);
			}

			Log.Debug("JSON report successfully created for database {DatabaseName}", databaseName);
		}

		private static string GetReportFilePath(string reportDirectoryPath, string databaseName)
		{
			var fileName = CreateFileName(databaseName);
			var sanitizedDirectoryPath = SanitizeReportDirectoryPath(reportDirectoryPath);

			return Path.Combine(sanitizedDirectoryPath, fileName);
		}

		private static string CreateFileName(string databaseName) => $"{databaseName}-{DateTime.UtcNow:yyyy-MM-dd_hh-mm-ss-zz}.json";

		private static string SanitizeReportDirectoryPath(string reportDirectoryPath)
		{
			return FileSystemHelpers.IsDirectory(reportDirectoryPath) ? 
				reportDirectoryPath : 
				Path.GetDirectoryName(reportDirectoryPath);
		}
	}
}
