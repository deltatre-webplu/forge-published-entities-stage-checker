using System.IO;

namespace ForgePublishedEntitiesStageChecker.Helpers
{
	public static class FileSystemHelpers
	{
		public static void EnsureFileDirectoryExists(string fileAbsolutePath)
		{
			var directoryPath = Path.GetDirectoryName(fileAbsolutePath);
			Directory.CreateDirectory(directoryPath);
		}
	}
}
