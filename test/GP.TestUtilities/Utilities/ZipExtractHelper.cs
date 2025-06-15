using System.IO.Compression;

namespace GP.TestUtilities.Utilities
{
    public static class ZipExtractHelper
    {
        public static string ExtractToTempDirectory(string zipFilePath, string? prefix = null)
        {
            if (!File.Exists(zipFilePath))
            { throw new FileNotFoundException("Zip file not found", zipFilePath); }

            var zipName = Path.GetFileNameWithoutExtension(zipFilePath);
            var dirPrefix = prefix ?? zipName;

            var extractDir = TempDirectoryHelper.CreateTempDirectory(dirPrefix);

            ZipFile.ExtractToDirectory(zipFilePath, extractDir);

            return extractDir;
        }
    }
}