using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace ForgePublishedEntitiesStageChecker.Mongo
{
	public class MongoCollectionFactory
	{
		private readonly Lazy<IMongoDatabase> _backendDatabase;

		public MongoCollectionFactory(string connString)
		{
			if (string.IsNullOrWhiteSpace(connString))
				throw new ArgumentException("Backend store connection string cannot be null or white space.", nameof(connString));

			_backendDatabase = new Lazy<IMongoDatabase>(() => GetDatabaseFromConnString(connString));
		}

		public IMongoCollection<BsonDocument> GetMongoCollection(string collectionName)
		{
			return _backendDatabase.Value.GetCollection<BsonDocument>(
				collectionName,
				new MongoCollectionSettings
				{
					GuidRepresentation = GuidRepresentation.CSharpLegacy
				});
		}

		private static IMongoDatabase GetDatabaseFromConnString(string connString)
		{
			var mongoUrl = MongoUrl.Create(connString);
			var databaseName = mongoUrl.DatabaseName;

			var mongoClient = new MongoClient(connString);
			var database = mongoClient.GetDatabase(databaseName);
			return database;
		}
	}
}
