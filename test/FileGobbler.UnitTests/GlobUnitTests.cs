using FileGlobber.Services;

namespace FileGobbler.UnitTests
{
    public class GlobUnitTests
    {
        [Fact]
        public void GlobCreate_ReturnsGlobber_WithValidConstruction()
        {
            // Prepare
            var testPath = @"C:\test\path";

            // Execute
            var result = Glob.Create(testPath);

            // Validate
            Assert.NotNull(result);
            Assert.IsType<Globber>(result);
        }
    }
}