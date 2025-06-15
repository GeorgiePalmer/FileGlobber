using FileGlobber.Models;
using FileGlobber.Services;
using FileGobbler.TestUtilities.Enums;
using FileGobbler.TestUtilities.Services;
using System.Reflection;

namespace FileGobbler.UnitTests
{
    public class GlobberTests
    {
        private TestDataHandler? _windowsTDHandler;
        private TestDataHandler? _linuxTDHandler;

        [Fact]
        public void Construct_WithOptions_ThrowsArgumentNullException_FromNullOptions()
        {
            // Prepare
            GlobOptions? testOptions = null;

            // Execute & Validate
            Assert.Throws<ArgumentNullException>(() => new Globber(testOptions!));
        }

        [Fact]
        public void Construct_Default_ReturnsGlobber_WithValidOptions()
        {
            // Prepare
            var testOptions = new GlobOptions();

            // Execute
            var result = new Globber(testOptions);

            // Validate
            Assert.NotNull(result);
            Assert.Equal(testOptions, result.Options);
        }

        [Fact]
        public void Construct_WithOptions_ReturnsGlobber_WithValidOptions()
        {
            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler?.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = [],
                MaxDepth = 50,
                IgnoreCase = false,
                IncludeHidden = false,
            };

            // Execute
            var result = new Globber(testOptions);

            // Validate
            Assert.NotNull(result);
            Assert.Equal(testOptions, result.Options);
        }

        [Fact]
        public void EnumerateDirectories_BasicExecution_ReturnsEnumerable_WithValidMatching()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare
            var testRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
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
            Assert.Equal(Directory.EnumerateDirectories(_windowsTDHandler.ExpectedData.RootPath, "*", SearchOption.AllDirectories).Count(), result.Count());
        }

        ~GlobberTests()
        {
            _windowsTDHandler?.Dispose();
            _linuxTDHandler?.Dispose();
        }
    }
}