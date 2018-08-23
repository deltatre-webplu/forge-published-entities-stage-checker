﻿using ForgePublishedEntitiesStageChecker.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace ForgePublishedEntitiesStageChecker.Mongo
{
	public class NeutralEntityStageChecker
	{
		private readonly IMongoCollection<BsonDocument> _publishedEntities;

		public NeutralEntityStageChecker(IMongoCollection<BsonDocument> publishedEntities)
		{
			_publishedEntities = publishedEntities ?? throw new ArgumentNullException(nameof(publishedEntities));
		}

		public async Task<ReadOnlyCollection<NeutralEntity>> GetPublishedEntitiesWithUnexpectedStageAsync()
		{
			var query = from document in this._publishedEntities.AsQueryable()
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
											Title = d["Title"]
										})
									};

			var queryItems = await query.ToListAsync().ConfigureAwait(false);

			var neutralEntities = queryItems.Select(item =>
			{
				var entityId = item.EntityId.AsGuid;
				var localizations = item.Localizations.Select(l =>
					new Localization
					{
						Culture = l.Culture.IsBsonNull ? null : l.Culture.AsString,
						Slug = l.Slug.IsBsonNull ? null : l.Slug.AsString,
						Title = l.Title.IsBsonNull ? null : l.Title.AsString,
						TranslationId = l.TranslationId.AsGuid
					})
					.ToList()
					.AsReadOnly();

				return new NeutralEntity
				{
					EntityId = entityId,
					Localizations = localizations
				};
			}).ToList().AsReadOnly();

			return neutralEntities;
		}
	}
}
