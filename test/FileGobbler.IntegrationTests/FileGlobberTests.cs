using FileGlobber.Models;
using FileGlobber.Services;
using FileGobbler.TestUtilities.Services;

namespace FileGobbler.IntegrationTests
{
    public class FileGlobberTests
    {
        private const int TOTAL_DIR_COUNT = 0;
        private const int TOTAL_FILE_COUNT = 0;

        private TestDataHandler _windowsTDHandler;
        private TestDataHandler _linuxTDHandler;

        public FileGlobberTests()
        {
            _windowsTDHandler = new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler = new TestDataHandler(TestDataHandler.TestDataKind.Linux);
        }

        [Fact]
        public void Run_EnumerateDirectories_Basic_Full()
        {
            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.Data.RootPath,
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

        ~FileGlobberTests()
        {
            _windowsTDHandler.Dispose();
            _linuxTDHandler.Dispose();
        }
    }
}