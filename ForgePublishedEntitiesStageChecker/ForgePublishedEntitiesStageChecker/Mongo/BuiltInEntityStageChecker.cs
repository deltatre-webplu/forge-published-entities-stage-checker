﻿using ForgePublishedEntitiesStageChecker.Contracts;
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
		private readonly string _entityType;
		private readonly string _tenantName;

		public BuiltInEntityStageChecker(IMongoCollection<BsonDocument> publishedEntitiesColl, string entityType, string tenantName)
		{
			if (string.IsNullOrWhiteSpace(entityType))
				throw new ArgumentException("Entity type cannot be null or white space", nameof(entityType));

			if (string.IsNullOrWhiteSpace(tenantName))
				throw new ArgumentException("Tenant name cannot be null or white space", nameof(tenantName));

			_publishedEntitiesColl = publishedEntitiesColl ?? throw new ArgumentNullException(nameof(publishedEntitiesColl));
			_entityType = entityType;
			_tenantName = tenantName;
		}

		public async Task<ReadOnlyCollection<Entity>> GetPublishedEntitiesWithUnexpectedStageAsync()
		{
			Log.Information("Executing query for entity {EntityType} and tenant {TenantName}...", _entityType, _tenantName);

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

			Log.Debug("Query for entity {EntityType} and tenant {TenantName} successfully completed", _entityType, _tenantName);

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

				return new Entity(entityId, _entityType, _entityType, localizations);
			})
			.ToList()
			.AsReadOnly();

			return entities;
		}
	}
}
