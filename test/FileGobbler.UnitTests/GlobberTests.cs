using FileGlobber.Models;
using FileGlobber.Services;
using FileGobbler.TestUtilities.Services;

namespace FileGobbler.UnitTests
{
    public class GlobberTests
    {
        private TestDataHandler _windowsTDHandler;
        private TestDataHandler _linuxTDHandler;

        public GlobberTests()
        {
            _windowsTDHandler = new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler = new TestDataHandler(TestDataHandler.TestDataKind.Linux);
        }

        [Fact]
        public void EnumerateDirectories_BasicExecution_ReturnsEnumerable_WithValidMatching()
        {
            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.Data.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = [],
                MaxDepth = 50,
                IgnoreCase = false,
                IncludeHidden = false,
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = globber.EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(Directory.EnumerateDirectories(_windowsTDHandler.Data.RootPath, "*", SearchOption.AllDirectories).Count(), result.Count());
        }

        ~GlobberTests()
        {
            _windowsTDHandler.Dispose();
            _linuxTDHandler.Dispose();
        }
    }
}