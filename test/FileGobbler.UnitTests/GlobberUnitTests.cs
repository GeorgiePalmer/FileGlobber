using FileGlobber.Models;
using FileGlobber.Services;
using System.Reflection;

namespace FileGobbler.UnitTests
{
    public class GlobberUnitTests
    {
        [Fact]
        public void EnumerateDirectories_BasicExecution_ReturnsEnumerable_WithValidMatching()
        {
            // Prepare
            var testRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testOptions = new GlobOptions()
            {
                RootPath = testRootPath,
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
            Assert.Equal(result.Count(), Directory.EnumerateDirectories(testRootPath).Count());
        }
    }
}