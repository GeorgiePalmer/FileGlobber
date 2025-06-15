using FileGlobber.Models;
using FileGlobber.Services;
using FileGobbler.TestUtilities.Enums;
using FileGobbler.TestUtilities.Services;

namespace FileGobbler.IntegrationTests.Support
{
    public class PatternSupportTests
    {
        private TestDataHandler? _windowsTDHandler;
        private TestDataHandler? _linuxTDHandler;

        #region Match Tests

        [Fact]
        public void Directories_MatchAll()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = globber.EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedDirectories.Length, result.Count());
        }

        [Fact]
        public void Directories_MatchStartWithA()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["a*/"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.StartsWith("a", d, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Directories_MatchEndWithData()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*data/"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.EndsWith("data", d, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Directories_MatchTwoLevelsDeep()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*/*/"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.Contains("/", d));
        }

        [Fact]
        public void Directories_MatchRecursiveGlob()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["**"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.True(result.Any());
        }

        [Fact]
        public void Directories_MatchWithUnderscore()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*_*/"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.Contains("_", d));
        }

        [Fact]
        public void Directories_MatchSingleCharacterNames()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["?/"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.Equal(1, d.TrimEnd('/').Length));
        }

        [Fact]
        public void Directories_MatchContainingDigits()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*[0-9]*/"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.Matches(@"\d", d));
        }

        [Fact]
        public void Directories_MatchIntermediateWildcard()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["src*/*/impl/"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.EndsWith("impl", d, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Directories_MatchHiddenOnly()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = [".*/"],
                ExcludePatterns = []
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.StartsWith(".", d));
        }

        #endregion Match Tests

        #region Exclude Tests

        [Fact]
        public void Directories_MatchAny_ExcludeAll()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
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

        [Fact]
        public void Directories_ExcludeStartWithA()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["a*/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.False(d.StartsWith("a", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void Directories_ExcludeEndWithData()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["*data/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.False(d.EndsWith("data", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void Directories_ExcludeTwoLevelsDeep()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["*/*/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.True(d.Count(c => c == '/' || c == '\\') < 2));
        }

        [Fact]
        public void Directories_ExcludeAllRecursive()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["**"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Directories_ExcludeWithUnderscore()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["*_*/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.DoesNotContain("_", d));
        }

        [Fact]
        public void Directories_ExcludeSingleCharacterNames()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["?/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.True(d.TrimEnd('/').Length != 1));
        }

        [Fact]
        public void Directories_ExcludeContainingDigits()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["*[0-9]*/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.DoesNotMatch(@"\d", d));
        }

        [Fact]
        public void Directories_ExcludeIntermediateImplFolders()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = ["*/impl/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.DoesNotContain("/impl", d));
        }

        [Fact]
        public void Directories_ExcludeHiddenOnly()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = [".*/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.False(d.StartsWith(".")));
        }

        #endregion Exclude Tests

        #region Mixed Tests

        [Fact]
        public void Directories_MatchAll_ExcludeWithStartCharA()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
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

        [Fact]
        public void Directories_MatchStartWithA_ExcludeTempSuffix()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["a*/"],
                ExcludePatterns = ["*-tmp/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.StartsWith("a", d, StringComparison.OrdinalIgnoreCase));
            Assert.All(result, d => Assert.False(d.EndsWith("-tmp", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void Directories_MatchEndWithData_ExcludeContainingOld()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*data/"],
                ExcludePatterns = ["*old*/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.EndsWith("data", d, StringComparison.OrdinalIgnoreCase));
            Assert.All(result, d => Assert.DoesNotContain("old", d, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Directories_MatchTwoLevelsDeep_ExcludeBin()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*/*/"],
                ExcludePatterns = ["bin/**"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.Contains("/", d));
            Assert.All(result, d => Assert.DoesNotContain("bin", d, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Directories_MatchAll_ExcludeHiddenAndObj()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"],
                ExcludePatterns = [".*/", "obj/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.False(d.StartsWith(".")));
            Assert.All(result, d => Assert.DoesNotContain("obj", d));
        }

        [Fact]
        public void Directories_MatchWithUnderscore_ExcludeTestSuffix()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*_*/"],
                ExcludePatterns = ["*_test/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.Contains("_", d));
            Assert.All(result, d => Assert.False(d.EndsWith("_test", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void Directories_MatchSingleChar_ExcludeZ()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["?/"],
                ExcludePatterns = ["z/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.Equal(1, d.TrimEnd('/').Length));
            Assert.All(result, d => Assert.NotEqual("z", d.TrimEnd('/')));
        }

        [Fact]
        public void Directories_MatchWithDigits_ExcludeTempPrefix()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*[0-9]*/"],
                ExcludePatterns = ["temp*/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.Matches(@"\d", d));
            Assert.All(result, d => Assert.False(d.StartsWith("temp", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void Directories_MatchImplNested_ExcludeLegacy()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["src*/foo/impl/"],
                ExcludePatterns = ["*legacy*/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.EndsWith("impl", d));
            Assert.All(result, d => Assert.DoesNotContain("legacy", d));
        }

        [Fact]
        public void Directories_MatchHidden_ExcludeDotGit()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = [".*/"],
                ExcludePatterns = [".git/"]
            };

            var globber = new Globber(testOptions);

            var result = globber.EnumerateDirectories();

            Assert.NotNull(result);
            Assert.All(result, d => Assert.StartsWith(".", d));
            Assert.All(result, d => Assert.NotEqual(".git", d.TrimEnd('/')));
        }

        #endregion Mixed Tests

        ~PatternSupportTests()
        {
            _windowsTDHandler?.Dispose();
            _linuxTDHandler?.Dispose();
        }
    }
}