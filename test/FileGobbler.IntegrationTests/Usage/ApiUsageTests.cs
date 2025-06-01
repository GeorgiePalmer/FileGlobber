using FileGlobber.Services;
using FileGobbler.TestUtilities.Services;

namespace FileGobbler.IntegrationTests.Usage
{
    public class ApiUsageTests
    {
        private TestDataHandler? _windowsTDHandler;
        private TestDataHandler? _linuxTDHandler;

        [Fact]
        public void Run_EnumerateDirectories_Api_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Linux);

            // Prepare & Execute
            var result = Glob.Create(_windowsTDHandler.Data.RootPath)
                .Match("*")
                .EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.Data.ExpectedDirectories.Length, result.Count());
        }

        [Fact]
        public void Run_EnumerateFiles_Api_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Linux);

            // Prepare & Execute
            var result = Glob.Create(_windowsTDHandler.Data.RootPath)
                .Match("*")
                .EnumerateFiles();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.Data.ExpectedFiles.Length, result.Count());
        }

        [Fact]
        public async Task Run_EnumerateDirectoriesAsync_Api_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Linux);

            // Prepare & Execute
            var result = await Glob.Create(_windowsTDHandler.Data.RootPath)
                .Match("*")
                .EnumerateDirectoriesAsync();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.Data.ExpectedDirectories.Length, result.Count());
        }

        [Fact]
        public async Task Run_EnumerateFilesAsync_Api_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Windows);
            _linuxTDHandler ??= new TestDataHandler(TestDataHandler.TestDataKind.Linux);

            // Prepare & Execute
            var result = await Glob.Create(_windowsTDHandler.Data.RootPath)
                .Match("*")
                .EnumerateFilesAsync();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.Data.ExpectedFiles.Length, result.Count());
        }

        ~ApiUsageTests()
        {
            _windowsTDHandler?.Dispose();
            _linuxTDHandler?.Dispose();
        }
    }
}