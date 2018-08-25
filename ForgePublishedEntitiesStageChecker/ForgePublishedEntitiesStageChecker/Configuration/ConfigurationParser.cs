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
		private const string ReportDirectoryPathConfigKey = "ReportDirectoryPath";

		public OperationResult<Settings, string> GetSettingsFromConfiguration(IConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			var mongoConnString = configuration[MongoConnStringConfigKey];
			var reportFilePath = configuration[ReportDirectoryPathConfigKey];

			var errors = CollectConfigurationErrors(mongoConnString, reportFilePath);
			if (errors.Count > 0)
				return OperationResult<Settings, string>.CreateFailure(errors.ToNonEmptySequence());

			var settings = new Settings(mongoConnString, reportFilePath);
			return OperationResult<Settings, string>.CreateSuccess(settings);
		}

		private static List<string> CollectConfigurationErrors(string mongoConnString, string reportDirectoryPath)
		{
			var errors = new List<string>();

			if (string.IsNullOrWhiteSpace(mongoConnString))
				errors.Add(MongoConnStringConfigKey);

			if (string.IsNullOrWhiteSpace(reportDirectoryPath))
				errors.Add(ReportDirectoryPathConfigKey);

			return errors;
		}
	}
}
