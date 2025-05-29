using FileGlobber.Services;

namespace FileGobbler.UnitTests
{
    public class GlobTests
    {
        [Fact]
        public void GlobCreate_ReturnsGlobber_WithValidPath()
        {
            // Prepare
            var testPath = @"C:\test\path";

            // Execute
            var result = Glob.Create(testPath);

            // Validate
            Assert.NotNull(result);
            Assert.IsType<Globber>(result);
        }

        [Fact]
        public void GlobCreate_ThrowsArgumentException_FromEmptyPath()
        {
            // Prepare
            var testPath = string.Empty;

            // Execute & Validate
            Assert.Throws<ArgumentException>(() => Glob.Create(testPath));
        }

        [Fact]
        public void GlobCreate_ThrowsArgumentException_FromNotQualifiedPath()
        {
            // Prepare
            var testPath = @"C\Not\Qualified\";

            // Execute & Validate
            Assert.Throws<ArgumentException>(() => Glob.Create(testPath));
        }

        [Fact]
        public void GlobCreate_ThrowsArgumentNullException_FromNullPath()
        {
            // Prepare
            string? testPath = null;

            // Execute & Validate
            Assert.Throws<ArgumentNullException>(() => Glob.Create(testPath));
        }
    }
}