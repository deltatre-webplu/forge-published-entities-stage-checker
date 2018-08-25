using ForgePublishedEntitiesStageChecker.Configuration;
using ForgePublishedEntitiesStageChecker.Contracts;
using ForgePublishedEntitiesStageChecker.Logging;
using ForgePublishedEntitiesStageChecker.Mongo;
using ForgePublishedEntitiesStageChecker.Report;
using Microsoft.Extensions.Configuration;
using Serilog;
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

		public async static Task Main(string[] args)
		{
			var configuration = ReadConfiguration(args);

			BootstrapLogger(configuration);

			Log.Debug("Start processing");

			try
			{
				await RunAsync(configuration).ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				Log.Error(exception, "An error occurred");
			}

			Log.Debug("End processing");

			Log.CloseAndFlush();
		}

		private static async Task RunAsync(IConfiguration configuration)
		{
			Log.Information("Reading command line arguments...");
			var configurationParser = new ConfigurationParser();
			var settingsReadingResult = configurationParser.GetSettingsFromConfiguration(configuration);
			if (!settingsReadingResult.IsSuccess)
			{
				LogErrorsForWrongConfiguration(settingsReadingResult.Errors);
				return;
			}
			var settings = settingsReadingResult.Output;

			Log.Debug("Provided command line arguments have been successfully validated");

			var collectionFactory = new MongoCollectionFactory(settings.MongoConnString);

			var publishedEntitiesWithUnexpectedStage = (await
				GetPublishedEntitiesWithUnexpectedStageAsync(collectionFactory).ConfigureAwait(false)).ToArray();

			var databaseName = collectionFactory.DatabaseName;

			Log.Information(
				"Found {NumberOfEntities} published entities with unexpected stage for database {DatabaseName}", 
				publishedEntitiesWithUnexpectedStage.Length,
				databaseName);

			ExportJsonReport(settings.ReportFilePath, publishedEntitiesWithUnexpectedStage, databaseName);
		}

		private static IConfiguration ReadConfiguration(string[] commandLineArgs)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddCommandLine(commandLineArgs)
				.Build();

			return config;
		}

		private static void LogErrorsForWrongConfiguration(IEnumerable<string> errors)
		{
			var missingConfigurations = string.Join(" ", errors);
			Log.Error("Missing mandatory configuration(s): {MissingConfigurations}", missingConfigurations);
		}

		private static void BootstrapLogger(IConfiguration configuration)
		{
			var loggerFactory = new LoggerFactory();
			Log.Logger = loggerFactory.CreateLogger(configuration);
		}

		private static async Task<IEnumerable<Entity>> GetPublishedEntitiesWithUnexpectedStageAsync(MongoCollectionFactory collectionFactory)
		{
			var taskFactories = BuiltInEntities.Select(CreateTaskFactory).Concat(new[] { CreateTaskFactoryForCustomEntities() } );
			var tasks = taskFactories.Select(factory => factory());
			var taskResults = await Task.WhenAll(tasks).ConfigureAwait(false);

			return taskResults.SelectMany(entities => entities);

			Func<Task<ReadOnlyCollection<Entity>>> CreateTaskFactory((string entityType, string collectionName) builtInEntity)
			{
				var collection = collectionFactory.GetMongoCollection(builtInEntity.collectionName);
				var stageChecker = new BuiltInEntityStageChecker(collection, builtInEntity.entityType);
				return () => stageChecker.GetPublishedEntitiesWithUnexpectedStageAsync();
			}

			Func<Task<ReadOnlyCollection<Entity>>> CreateTaskFactoryForCustomEntities()
			{
				var collection = collectionFactory.GetMongoCollection(CustomEntitiesCollection);
				var stageChecker = new CustomEntityStageChecker(collection);
				return () => stageChecker.GetPublishedEntitiesWithUnexpectedStageAsync();
			}
		}

		private static void ExportJsonReport(
			string reportFilePath, 
			IEnumerable<Entity> publishedEntitiesWithUnexpectedStage,
			string databaseName)
		{
			var reportCreator = new ReportCreator();
			reportCreator.CreateJsonReport(reportFilePath, publishedEntitiesWithUnexpectedStage, databaseName);
		}
	}
}
