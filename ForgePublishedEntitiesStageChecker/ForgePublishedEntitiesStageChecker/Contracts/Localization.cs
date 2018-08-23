using System;

namespace ForgePublishedEntitiesStageChecker.Contracts
{
	public class Localization
	{
		public Guid TranslationId { get; set; }
		public string Slug { get; set; }
		public string Title { get; set; }
		public string Culture { get; set; }
	}
}
