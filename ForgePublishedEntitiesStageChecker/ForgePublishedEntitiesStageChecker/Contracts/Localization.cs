using System;

namespace ForgePublishedEntitiesStageChecker.Contracts
{
	public class Localization
	{
		public Guid TranslationId { get; }
		public string Slug { get; }
		public string Title { get; }
		public string Culture { get; }
		public string Stage { get; }

		public Localization(Guid translationId, string slug, string title, string culture, string stage)
		{
			this.TranslationId = translationId;
			this.Slug = slug;
			this.Title = title;
			this.Culture = culture;
			this.Stage = stage;
		}
	}
}
