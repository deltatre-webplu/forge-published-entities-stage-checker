using System;

namespace ForgePublishedEntitiesStageChecker.Contracts
{
	public class Localization
	{
		public Guid TranslationId { get; }
		public string Slug { get; }
		public string Title { get; }
		public string Culture { get; }

		public Localization(Guid translationId, string slug, string title, string culture)
		{
			this.TranslationId = translationId;
			this.Slug = slug;
			this.Title = title;
			this.Culture = culture;
		}
	}
}
