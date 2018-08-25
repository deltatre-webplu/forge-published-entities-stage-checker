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
			string tenantName)
		{
			Log.Information("Creating JSON report for tenant {TenantName}...", tenantName);

			var reportFilePath = GetReportFilePath(reportDirectoryPath, tenantName);
			Log.Debug("Computed report file absolute path for tenant {TenantName} is {ReportFilePath}", tenantName, reportFilePath);
			FileSystemHelpers.EnsureFileDirectoryExists(reportFilePath);

			using (var streamWriter = File.CreateText(reportFilePath))
			{
				var serializer = new JsonSerializer
				{
					Formatting = Formatting.Indented
				};
				serializer.Serialize(streamWriter, publishedEntitiesWithUnexpectedStage);
			}

			Log.Debug("JSON report successfully created for tenant {TenantName}", tenantName);
		}

		private static string GetReportFilePath(string reportDirectoryPath, string databaseName)
		{
			var fileName = CreateFileName(databaseName);
			return Path.Combine(reportDirectoryPath, fileName);
		}

		private static string CreateFileName(string databaseName) => $"{databaseName}-{DateTime.Now:yyyy-MM-dd_hh-mm-ss-fffzz}.json";
	}
}
