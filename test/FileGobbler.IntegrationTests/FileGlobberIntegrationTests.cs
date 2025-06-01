using FileGlobber.Models;
using FileGlobber.Services;
using FileGobbler.TestUtilities.Services;

namespace FileGobbler.IntegrationTests
{
    public class FileGlobberIntegrationTests
    {
        private TestDataHandler _windowsTDHandler;
        private TestDataHandler _linuxTDHandler;

        public FileGlobberIntegrationTests()
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
            Assert.Equal(_windowsTDHandler.Data.ExpectedDirectories.Length, result.Count());
        }

        ~FileGlobberIntegrationTests()
        {
            _windowsTDHandler.Dispose();
            _linuxTDHandler.Dispose();
        }
    }
}