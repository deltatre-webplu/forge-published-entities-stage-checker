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

			var errors = new List<string>();

			if (string.IsNullOrWhiteSpace(configuration[MongoConnStringConfigKey]))
				errors.Add($"Missing mandatory configuration: {MongoConnStringConfigKey}");

			if (string.IsNullOrWhiteSpace(configuration[LogFilePathConfigKey]))
				errors.Add($"Missing mandatory configuration: {LogFilePathConfigKey}");

			if (string.IsNullOrWhiteSpace(configuration[ReportFilePathConfigKey]))
				errors.Add($"Missing mandatory configuration: {ReportFilePathConfigKey}");

			return errors.Count == 0
				? ValidationResult<IConfiguration, string>.CreateValid(configuration)
				: ValidationResult<IConfiguration, string>.CreateInvalid(errors.ToNonEmptySequence());
		}
	}
}
