using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ForgePublishedEntitiesStageChecker
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
			.AddCommandLine(args)
			.Build();

			var configValidator = new ConfigurationValidator();
			var configValidationResult = configValidator.Validate(config);
			if (!configValidationResult.IsValid)
			{
				ShowMessageForMissingConfigurations(configValidationResult.Errors);
				return;
			}

			Console.WriteLine("Hello my friend !");
		}

		private static void ShowMessageForMissingConfigurations(IEnumerable<string> errors)
		{
			var message = string.Join(Environment.NewLine, errors);
			Console.WriteLine(message);
			Console.WriteLine();

			Console.WriteLine("Press enter to close...");
			Console.ReadLine();
		}
	}
}
