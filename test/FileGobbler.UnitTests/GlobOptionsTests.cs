namespace FileGobbler.UnitTests
{
    public class GlobOptionsTests
    {
        [Fact]
        public void Construct_Default_HasValidDefaultValues()
        {
            // Prepare
            var options = new GlobOptions();

            // Validate
            Assert.NotNull(options);
            Assert.Equal(string.Empty, options.RootPath);
            Assert.Empty(options.MatchPatterns);
            Assert.Empty(options.ExcludePatterns);
            Assert.Equal((uint)50, options.MaxDepth);
            Assert.False(options.IgnoreCase);
            Assert.False(options.IncludeHidden);
            Assert.Equal($"{Path.DirectorySeparatorChar}", options.NormalizedRoot);
            Assert.Equal(1, options.PrefixLength);
        }

        [Fact]
        public void Construct_WithSetters_HasValidValues()
        {
            // Prepare
            var rootPath = "C:\\TestRoot/\\/";
            List<string> matchPatterns = ["*.txt", "*.md"];
            List<string> excludePatterns = ["*.tmp"];
            uint maxDepth = 10;

            var options = new GlobOptions
            {
                RootPath = rootPath,
                MatchPatterns = matchPatterns,
                ExcludePatterns = excludePatterns,
                MaxDepth = maxDepth,
                IgnoreCase = true,
                IncludeHidden = true
            };

            // Validate
            var expectNormalRoot = "C:\\TestRoot\\";
            Assert.NotNull(options);
            Assert.Equal(rootPath, options.RootPath);
            Assert.Equal(matchPatterns, options.MatchPatterns);
            Assert.Equal(excludePatterns, options.ExcludePatterns);
            Assert.Equal(maxDepth, options.MaxDepth);
            Assert.True(options.IgnoreCase);
            Assert.True(options.IncludeHidden);
            Assert.Equal(expectNormalRoot, options.NormalizedRoot);
            Assert.Equal(expectNormalRoot.Length, options.PrefixLength);
        }

        [Fact]
        public void ValidatePatterns_TrimDupePatterns_ReturnsVoid_WithSuccessfulValidation()
        {
            // Prepare
            var options = new GlobOptions
            {
                MatchPatterns = ["*.txt", "*.txt"],
                ExcludePatterns = ["*.md", "*.md"]
            };
            // Execute
            options.ValidatePatterns();

            // Validate
            Assert.Single(options.MatchPatterns);
            Assert.Single(options.ExcludePatterns);
        }

        [Fact]
        public void ValidatePatterns_RemoveOverlapPatterns_ReturnsVoid_WithSuccessfulValidation()
        {
            // Prepare
            var options = new GlobOptions
            {
                MatchPatterns = ["*.md", "*.unique"],
                ExcludePatterns = ["*.md"]
            };
            // Execute
            options.ValidatePatterns();

            // Validate
            Assert.Single(options.MatchPatterns);
            Assert.Empty(options.ExcludePatterns);
        }

        [Fact]
        public void ValidatePatterns_ThrowsArgumentException_FromNoMatchPatterns()
        {
            // Prepare
            var options = new GlobOptions
            {
                ExcludePatterns = ["*.md"]
            };

            // Execute & Validate
            var ex = Assert.Throws<ArgumentException>(options.ValidatePatterns);
            Assert.Contains("No match patterns have been specified.", ex.Message);
        }

        [Fact]
        public void ValidatePatterns_ThrowsArgumentException_FromTotalOverlapPatterns()
        {
            // Prepare
            var options = new GlobOptions
            {
                MatchPatterns = ["*.md"],
                ExcludePatterns = ["*.md"]
            };

            // Execute & Validate
            var ex = Assert.Throws<ArgumentException>(options.ValidatePatterns);
            Assert.Contains("Match and exclude patterns cannot completely overlap.", ex.Message);
        }
    }
}