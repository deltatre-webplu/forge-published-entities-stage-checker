using System;

namespace ForgePublishedEntitiesStageChecker.Contracts
{
	public class Localization
	{
		public Guid TranslationId { get; set; }
		public string Slug { get; set; }
		public string Title { get; set; }
		public string Culture { get; set; }

		public Localization(Guid translationId, string slug, string title, string culture)
		{
			this.TranslationId = translationId;
			this.Slug = slug;
			this.Title = title;
			this.Culture = culture;
		}
	}
}
