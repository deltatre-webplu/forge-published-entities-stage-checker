using Deltatre.Utils.Dto;
using Deltatre.Utils.Extensions.Enumerable;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ForgePublishedEntitiesStageChecker
{
	public class ConfigurationValidator
	{
		private const string MongoConnStringConfigKey = "MongoConnString";
		private const string LogFilePathConfigKey = "LogFilePath";
		private const string ReportFilePathConfigKey = "ReportFilePath";

		public ValidationResult<IConfiguration, string> Validate(IConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			var missingConfigurations = new List<string>();

			if (string.IsNullOrWhiteSpace(configuration[MongoConnStringConfigKey]))
				missingConfigurations.Add(MongoConnStringConfigKey);

			if (string.IsNullOrWhiteSpace(configuration[LogFilePathConfigKey]))
				missingConfigurations.Add(LogFilePathConfigKey);

			if (string.IsNullOrWhiteSpace(configuration[ReportFilePathConfigKey]))
				missingConfigurations.Add(ReportFilePathConfigKey);

			if (missingConfigurations.Count == 0)
				return ValidationResult<IConfiguration, string>.CreateValid(configuration);

			var errorMessage = $"Please provide following configuration(s): {string.Join(" ", missingConfigurations)}";
			var errors = new[] { errorMessage }.ToNonEmptySequence();
			return ValidationResult<IConfiguration, string>.CreateInvalid(errors);
		}
	}
}
