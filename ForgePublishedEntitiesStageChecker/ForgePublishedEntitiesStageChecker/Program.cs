using ForgePublishedEntitiesStageChecker.Mongo;
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

			var client = new MongoClient(config["MongoConnString"]);
			var db = client.GetDatabase("forge");
			var coll = db.GetCollection<BsonDocument>("wcm.TagsPublished", new MongoCollectionSettings { GuidRepresentation = GuidRepresentation.CSharpLegacy });

			var checker = new NeutralEntityStageChecker(coll);
			var entities = await checker.GetPublishedEntitiesWithUnexpectedStageAsync().ConfigureAwait(false);

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
