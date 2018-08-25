using Newtonsoft.Json;

namespace ForgePublishedEntitiesStageChecker.Contracts
{
	public class Tenant
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "connString")]
		public string ConnString { get; set; }
	}
}
