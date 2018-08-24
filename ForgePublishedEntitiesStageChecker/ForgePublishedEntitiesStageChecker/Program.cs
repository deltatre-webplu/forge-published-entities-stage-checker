using ForgePublishedEntitiesStageChecker.Configuration;
using ForgePublishedEntitiesStageChecker.Mongo;
using ForgePublishedEntitiesStageChecker.Report;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForgePublishedEntitiesStageChecker
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			RunAsync(args).Wait();
		}

		private static async Task RunAsync(string[] args)
		{
			var configuration = ReadConfiguration(args);

			var configurationParser = new ConfigurationParser();
			var parsingResult = configurationParser.ParseConfiguration(configuration);
			if (!parsingResult.IsSuccess)
			{
				ShowMessageForConfigurationErrors(parsingResult.Errors);
				return;
			}

			var client = new MongoClient(configuration["MongoConnString"]);
			var db = client.GetDatabase("forge");
			var coll = db.GetCollection<BsonDocument>("wcm.TagsPublished", new MongoCollectionSettings { GuidRepresentation = GuidRepresentation.CSharpLegacy });

			var checker = new BuiltInEntityStageChecker(coll);
			var entities = await checker.GetPublishedEntitiesWithUnexpectedStageAsync("tag").ConfigureAwait(false);

			var reportCreator = new ReportCreator();
			reportCreator.CreateJsonReport(parsingResult.Output.ReportFilePath, entities);

			Console.WriteLine("All done here !");
		}

		private static IConfiguration ReadConfiguration(string[] commandLineArgs)
		{
			var config = new ConfigurationBuilder()
			.AddCommandLine(commandLineArgs)
			.Build();

			return config;
		}

		private static void ShowMessageForConfigurationErrors(IEnumerable<string> errors)
		{
			var message = string.Join(Environment.NewLine, errors);
			Console.WriteLine(message);
			Console.WriteLine();

			Console.WriteLine("Press enter to close...");
			Console.ReadLine();
		}
	}
}
