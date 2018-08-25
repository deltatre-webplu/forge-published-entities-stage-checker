using ForgePublishedEntitiesStageChecker.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ForgePublishedEntitiesStageChecker.Configuration
{
	public class TenantConfigurationReader
	{
		public ReadOnlyCollection<Tenant> ReadTenantsFromFile(string filePath)
		{
			Log.Information("Reading tenants from file {FilePath}...", filePath);

			using (var streamReader = File.OpenText(filePath))
			{
				var serializer = new JsonSerializer();
				var tenants = (IEnumerable<Tenant>)serializer.Deserialize(streamReader, typeof(IEnumerable<Tenant>));

				return new ReadOnlyCollection<Tenant>(tenants.ToList());
			}
		}
	}
}
