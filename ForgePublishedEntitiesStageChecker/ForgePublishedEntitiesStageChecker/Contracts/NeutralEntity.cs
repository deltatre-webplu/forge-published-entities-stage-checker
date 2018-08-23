using System;
using System.Collections.ObjectModel;

namespace ForgePublishedEntitiesStageChecker.Contracts
{
	public class NeutralEntity
	{
		public Guid EntityId { get; set; }
		public ReadOnlyCollection<Localization> Localizations { get; set; }
	}
}
