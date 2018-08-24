using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ForgePublishedEntitiesStageChecker.Contracts
{
	public class NeutralEntity
	{
		public Guid EntityId { get; }
		public ReadOnlyCollection<Localization> Localizations { get; }

		public NeutralEntity(Guid entityId, IEnumerable<Localization> localizations)
		{
			if (localizations == null)
				throw new ArgumentNullException(nameof(localizations));

			this.EntityId = entityId;
			this.Localizations = new ReadOnlyCollection<Localization>(localizations.ToList());
		}
	}
}
