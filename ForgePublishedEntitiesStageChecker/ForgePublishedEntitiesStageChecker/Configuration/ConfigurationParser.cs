using Deltatre.Utils.Dto;
using Deltatre.Utils.Extensions.Enumerable;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ForgePublishedEntitiesStageChecker.Configuration
{
	public class ConfigurationParser
	{
		private const string MongoConnStringConfigKey = "MongoConnString";
		private const string LogFilePathConfigKey = "LogFilePath";
		private const string ReportFilePathConfigKey = "ReportFilePath";

		public OperationResult<Settings, string> ParseConfiguration(IConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			var mongoConnString = configuration[MongoConnStringConfigKey];
			var logFilePath = configuration[LogFilePathConfigKey];
			var reportFilePath = configuration[ReportFilePathConfigKey];

			var errors = CollectConfigurationErrors(mongoConnString, logFilePath, reportFilePath);
			if (errors.Count > 0)
				return OperationResult<Settings, string>.CreateFailure(errors.ToNonEmptySequence());

			var settings = new Settings(mongoConnString, logFilePath, reportFilePath);
			return OperationResult<Settings, string>.CreateSuccess(settings);
		}

		private static List<string> CollectConfigurationErrors(string mongoConnString, string logFilePath, string reportFilePath)
		{
			var errors = new List<string>();

			if (string.IsNullOrWhiteSpace(mongoConnString))
				errors.Add($"Missing mandatory configuration: {MongoConnStringConfigKey}");

			if (string.IsNullOrWhiteSpace(logFilePath))
				errors.Add($"Missing mandatory configuration: {LogFilePathConfigKey}");

			if (string.IsNullOrWhiteSpace(reportFilePath))
				errors.Add($"Missing mandatory configuration: {ReportFilePathConfigKey}");

			return errors;
		}
	}
}
