using Deltatre.Utils.Dto;
using Deltatre.Utils.Extensions.Enumerable;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;

namespace ForgePublishedEntitiesStageChecker.Configuration
{
	public class ConfigurationParser
	{
		private const string ConfigFilePathKey = "ConfigFilePath";
		private const string ReportDirectoryPathKey = "ReportDirectoryPath";

		public OperationResult<Settings, string> GetSettingsFromConfiguration(IConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			Log.Information("Parsing command line arguments...");

			var configFilePath = configuration[ConfigFilePathKey];
			var reportDirectoryPath = configuration[ReportDirectoryPathKey];

			var errors = CollectConfigurationErrors(configFilePath, reportDirectoryPath);
			if (errors.Count > 0)
				return OperationResult<Settings, string>.CreateFailure(errors.ToNonEmptySequence());

			Log.Debug("Command line arguments have been successfully validated");
			var settings = new Settings(configFilePath, reportDirectoryPath);
			return OperationResult<Settings, string>.CreateSuccess(settings);
		}

		private static List<string> CollectConfigurationErrors(string configFilePath, string reportDirectoryPath)
		{
			var errors = new List<string>();

			if (string.IsNullOrWhiteSpace(configFilePath))
				errors.Add(ConfigFilePathKey);

			if (string.IsNullOrWhiteSpace(reportDirectoryPath))
				errors.Add(ReportDirectoryPathKey);

			return errors;
		}
	}
}
