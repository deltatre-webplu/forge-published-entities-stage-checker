using ForgePublishedEntitiesStageChecker.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ForgePublishedEntitiesStageChecker.Configuration
{
	public class TenantConfigurationReader
	{
		public ReadOnlyCollection<Tenant> ReadTenantsFromFile(string tenantFilePath)
		{
			using (var streamReader = File.OpenText(tenantFilePath))
			{
				var serializer = new JsonSerializer();
				var tenants = (IEnumerable<Tenant>)serializer.Deserialize(streamReader, typeof(IEnumerable<Tenant>));

				return new ReadOnlyCollection<Tenant>(tenants.ToList());
			}
		}
	}
}
