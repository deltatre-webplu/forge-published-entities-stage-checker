using ForgePublishedEntitiesStageChecker.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ForgePublishedEntitiesStageChecker.Mongo
{
	public class NeutralEntityStageChecker
	{
		private readonly IMongoCollection<BsonDocument> _publishedEntities;

		public NeutralEntityStageChecker(IMongoCollection<BsonDocument> publishedEntities)
		{
			_publishedEntities = publishedEntities ?? throw new ArgumentNullException(nameof(publishedEntities));
		}

		public int GetPublishedEntitiesWithUnexpectedStage()
		{
			var query = from document in this._publishedEntities.AsQueryable()
									where document["Stage"] == "reviewed" || document["Stage"] == "unpublished"
									group document by document["EntityId"] into g
									select new
									{
										EntityId = g.Key,
										Localizations = g.Select(i => new { TranslationId = i["_id"], Slug = i["Slug"], Culture = i["TranslationInfo.Culture"], Title = i["Title"] }).Distinct()
									};

			var data = query.ToList();
			return data.Count;
		}
	}
}
