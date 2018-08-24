using ForgePublishedEntitiesStageChecker.Configuration;
using ForgePublishedEntitiesStageChecker.Contracts;
using ForgePublishedEntitiesStageChecker.Mongo;
using ForgePublishedEntitiesStageChecker.Report;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ForgePublishedEntitiesStageChecker
{
	public static class Program
	{
		private readonly static (string entityType, string collectionName)[] BuiltInEntities = 
		{
			("album", "wcm.AlbumsPublished"),
			("document", "wcm.DocumentsPublished"),
			("photo", "wcm.PhotosPublished"),
			("selection", "wcm.SelectionsPublished"),
			("story", "wcm.StoriesPublished"),
			("tag", "wcm.TagsPublished")
		};

		private const string CustomEntitiesCollection = "wcm.CustomEntitiesPublished";

		public static void Main(string[] args)
		{
			RunAsync(args).Wait();
		}

		private static async Task RunAsync(string[] args)
		{
			var configuration = ReadConfiguration(args);

			var configurationParser = new ConfigurationParser();
			var settingsReadingResult = configurationParser.GetSettingsFromConfiguration(configuration);
			if (!settingsReadingResult.IsSuccess)
			{
				ShowMessageForConfigurationErrors(settingsReadingResult.Errors);
				return;
			}
			var settings = settingsReadingResult.Output;

			var publishedEntitiesWithUnexpectedStage = await
				GetPublishedEntitiesWithUnexpectedStageAsync(settings.MongoConnString).ConfigureAwait(false);

			ExportJsonReport(settings.ReportFilePath, publishedEntitiesWithUnexpectedStage);

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

		private static async Task<IEnumerable<Entity>> GetPublishedEntitiesWithUnexpectedStageAsync(string mongoConnString)
		{
			var collectionFactory = new MongoCollectionFactory(mongoConnString);

			var taskFactories = BuiltInEntities.Select(CreateTaskFactory).Concat(new[] { CreateTaskFactoryForCustomEntities() } );
			var tasks = taskFactories.Select(factory => factory());
			var taskResults = await Task.WhenAll(tasks).ConfigureAwait(false);

			return taskResults.SelectMany(entities => entities);

			Func<Task<ReadOnlyCollection<Entity>>> CreateTaskFactory((string entityType, string collectionName) builtInEntity)
			{
				var collection = collectionFactory.GetMongoCollection(builtInEntity.collectionName);
				var stageChecker = new BuiltInEntityStageChecker(collection);
				return () => stageChecker.GetPublishedEntitiesWithUnexpectedStageAsync(builtInEntity.entityType);
			}

			Func<Task<ReadOnlyCollection<Entity>>> CreateTaskFactoryForCustomEntities()
			{
				var collection = collectionFactory.GetMongoCollection(CustomEntitiesCollection);
				var stageChecker = new CustomEntityStageChecker(collection);
				return () => stageChecker.GetPublishedEntitiesWithUnexpectedStageAsync();
			}
		}

		private static void ExportJsonReport(string reportFilePath, IEnumerable<Entity> publishedEntitiesWithUnexpectedStage)
		{
			var reportCreator = new ReportCreator();
			reportCreator.CreateJsonReport(reportFilePath, publishedEntitiesWithUnexpectedStage);
		}
	}
}
