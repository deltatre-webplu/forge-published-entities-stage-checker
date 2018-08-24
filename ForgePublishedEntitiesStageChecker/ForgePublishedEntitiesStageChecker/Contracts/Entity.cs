using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ForgePublishedEntitiesStageChecker.Contracts
{
	public class Entity
	{
		public Guid EntityId { get; }
		public string EntityType { get; }
		public string EntityCode { get; }
		public ReadOnlyCollection<Localization> Localizations { get; }

		public Entity(Guid entityId, string entityType, string entityCode, IEnumerable<Localization> localizations)
		{
			if (localizations == null)
				throw new ArgumentNullException(nameof(localizations));

			this.EntityId = entityId;
			this.EntityType = entityType;
			this.EntityCode = entityCode;
			this.Localizations = new ReadOnlyCollection<Localization>(localizations.ToList());
		}
	}
}
