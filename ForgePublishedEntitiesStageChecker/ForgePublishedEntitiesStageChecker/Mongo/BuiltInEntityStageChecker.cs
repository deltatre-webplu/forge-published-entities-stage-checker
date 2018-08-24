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
	public class BuiltInEntityStageChecker
	{
		private readonly IMongoCollection<BsonDocument> _publishedEntitiesColl;

		public BuiltInEntityStageChecker(IMongoCollection<BsonDocument> publishedEntitiesColl)
		{
			_publishedEntitiesColl = publishedEntitiesColl ?? throw new ArgumentNullException(nameof(publishedEntitiesColl));
		}

		public async Task<ReadOnlyCollection<Entity>> GetPublishedEntitiesWithUnexpectedStageAsync(string entityType)
		{
			Log.Information("Executing query for entity {EntityType}...", entityType);

			var query = from document in this._publishedEntitiesColl.AsQueryable()
									where document["Stage"] == "reviewed" || document["Stage"] == "unpublished"
									group document by document["EntityId"] into g
									select new
									{
										EntityId = g.Key,
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

			Log.Debug("Query for entity {EntityType} successfully completed", entityType);

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

				return new Entity(entityId, entityType, entityType, localizations);
			})
			.ToList()
			.AsReadOnly();

			return entities;
		}
	}
}
