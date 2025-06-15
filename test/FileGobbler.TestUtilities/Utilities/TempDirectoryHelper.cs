using System.Reflection;

namespace FileGobbler.TestUtilities.Utilities
{
    public static class TempDirectoryHelper
    {
        public static string CreateTempDirectory(string? prefix = null)
        {
            var execDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            var projectName = prefix
                ?? Assembly.GetEntryAssembly()?.GetName().Name
                ?? TestUtilitiesConstants.DEFAULT_TEMP_DIR_PREFIX;

            var dirName = $"{projectName}_{Guid.NewGuid()}";

            var fullPath = Path.Combine(execDir!, dirName);
            Directory.CreateDirectory(fullPath);
            return fullPath;
        }
    }

    public sealed class TempDirectory : IDisposable
    {
        public string Path { get; }

        public TempDirectory(string? prefix = null)
        {
            Path = TempDirectoryHelper.CreateTempDirectory(prefix);
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(Path))
                { Directory.Delete(Path, recursive: true); }
            }
            catch // swallow any cleanup errors (tests shouldn’t fail on delete)
            { }
        }
    }
}