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

		public static bool IsDirectory(string path)
		{
			var attributes = File.GetAttributes(path);
			var isDirectory = attributes.HasFlag(FileAttributes.Directory);
			return isDirectory;
		}
	}
}
