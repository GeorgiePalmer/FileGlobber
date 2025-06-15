using FileGobbler.TestUtilities.Enums;
using FileGobbler.TestUtilities.Services;
using GP.FileGlobber.Services;

namespace FileGobbler.IntegrationTests.Usage
{
    public class ApiUsageTests
    {
        private TestDataHandler? _windowsTDHandler;
        private TestDataHandler? _linuxTDHandler;

        [Fact]
        public void Run_EnumerateDirectories_Api_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare & Execute
            var result = Glob.Create(_windowsTDHandler.ExpectedData.RootPath)
                .Match("*")
                .EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedDirectories.Length, result.Count());
        }

        [Fact]
        public void Run_EnumerateFiles_Api_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare & Execute
            var result = Glob.Create(_windowsTDHandler.ExpectedData.RootPath)
                .Match("*")
                .EnumerateFiles();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedFiles.Length, result.Count());
        }

        [Fact]
        public async Task Run_EnumerateDirectoriesAsync_Api_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare & Execute
            var result = await Glob.Create(_windowsTDHandler.ExpectedData.RootPath)
                .Match("*")
                .EnumerateDirectoriesAsync();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedDirectories.Length, result.Count());
        }

        [Fact]
        public async Task Run_EnumerateFilesAsync_Api_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare & Execute
            var result = await Glob.Create(_windowsTDHandler.ExpectedData.RootPath)
                .Match("*")
                .EnumerateFilesAsync();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedFiles.Length, result.Count());
        }

        ~ApiUsageTests()
        {
            _windowsTDHandler?.Dispose();
            _linuxTDHandler?.Dispose();
        }
    }
}