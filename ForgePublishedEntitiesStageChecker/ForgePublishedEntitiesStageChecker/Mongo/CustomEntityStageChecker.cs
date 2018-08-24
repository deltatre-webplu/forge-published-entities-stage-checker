using ForgePublishedEntitiesStageChecker.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Serilog;

namespace ForgePublishedEntitiesStageChecker.Mongo
{
	public class CustomEntityStageChecker
	{
		private const string EntityType = "customentity";

		private readonly IMongoCollection<BsonDocument> _publishedEntitiesColl;

		public CustomEntityStageChecker(IMongoCollection<BsonDocument> publishedEntitiesColl)
		{
			_publishedEntitiesColl = publishedEntitiesColl ?? throw new ArgumentNullException(nameof(publishedEntitiesColl));
		}

		public async Task<ReadOnlyCollection<Entity>> GetPublishedEntitiesWithUnexpectedStageAsync()
		{
			Log.Information("Executing query for {EntityType}...", "custom entities");

			var query = from document in this._publishedEntitiesColl.AsQueryable()
									where document["Stage"] == "reviewed" || document["Stage"] == "unpublished"
									group document by new { EntityId = document["EntityId"], EntityCode = document["EntityCode"] } into g
									select new
									{
										EntityId = g.Key.EntityId,
										EntityCode = g.Key.EntityCode,
										Localizations = g.Select(d => new
										{
											TranslationId = d["_id"],
											Slug = d["Slug"],
											Culture = d["TranslationInfo.Culture"],
											Title = d["Title"],
											Stage = d["Stage"]
										})
									};

			var queryItems = await query.ToListAsync().ConfigureAwait(false);

			Log.Debug("Query for {EntityType} successfully completed", "custom entities");

			var entities = queryItems.Select(item =>
			{
				var entityId = item.EntityId.AsGuid;

				var localizations = item.Localizations.Select(l =>
				{
					var culture = l.Culture.IsBsonNull ? null : l.Culture.AsString;
					var slug = l.Slug.IsBsonNull ? null : l.Slug.AsString;
					var title = l.Title.IsBsonNull ? null : l.Title.AsString;
					var translationId = l.TranslationId.AsGuid;
					var stage = l.Stage.AsString;

					return new Localization(translationId, slug, title, culture, stage);
				});

				var entityCode = item.EntityCode.IsBsonNull ? null : item.EntityCode.AsString;

				return new Entity(entityId, EntityType, entityCode, localizations);
			})
			.ToList()
			.AsReadOnly();

			return entities;
		}
	}
}
