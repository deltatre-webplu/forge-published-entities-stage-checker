using Microsoft.Extensions.Configuration;
using System;

namespace ForgePublishedEntitiesStageChecker
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables()
			.AddCommandLine(args)
			.Build();

			Console.WriteLine(config["Message"]);
			Console.ReadLine();
		}
	}
}
