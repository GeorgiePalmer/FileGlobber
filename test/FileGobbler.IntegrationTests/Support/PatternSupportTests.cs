using FileGlobber.Models;
using FileGlobber.Services;
using FileGobbler.TestUtilities.Services;

namespace FileGobbler.IntegrationTests.Support
{
    public class PatternSupportTests
    {
        private TestDataHandler? _windowsTDHandler;
        private TestDataHandler? _linuxTDHandler;

        #region Match Tests

        [Fact]
        public void Directories_MatchAll_ExcludeNone()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Linux);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.Data.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = globber.EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.Data.ExpectedDirectories.Length, result.Count());
        }

        #endregion Match Tests

        #region Exclude Tests

        [Fact]
        public void Directories_MatchAny_ExcludeAll()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Linux);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.Data.RootPath,
                MatchPatterns = ["Any"],
                ExcludePatterns = ["*"]
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = globber.EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion Exclude Tests

        #region Mixed Tests

        [Fact]
        public void Directories_MatchAll_ExcludeWithStartCharA()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Linux);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.Data.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["a/*"]
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = globber.EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(18, result.Count());
        }

        #endregion Mixed Tests

        ~PatternSupportTests()
        {
            _windowsTDHandler?.Dispose();
            _linuxTDHandler?.Dispose();
        }
    }
}