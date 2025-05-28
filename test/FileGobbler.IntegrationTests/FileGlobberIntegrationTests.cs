using RecursiveFileGlobber.Models;
using RecursiveFileGlobber.Services;
using System.Reflection;

namespace FileGobbler.IntegrationTests
{
    public class FileGlobberIntegrationTests
    {
        private static readonly string TEST_DATA_PATH = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName, "test-data");
        private const int TOTAL_DIR_COUNT = 0;
        private const int TOTAL_FILE_COUNT = 0;

        [Fact]
        public void Run_EnumerateDirectories_Basic_Full()
        {
            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = TEST_DATA_PATH,
                MatchPatterns = ["*"],
                ExcludePatterns = [],
                MaxDepth = 50,
                IgnoreCase = false,
                IncludeHidden = false
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = globber.EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(TOTAL_DIR_COUNT, result.Count());
        }
    }
}