namespace FileGobbler.IntegrationTests.Usage
{
    public class BasicUsageTests
    {
        private TestDataHandler? _windowsTDHandler;
        private TestDataHandler? _linuxTDHandler;

        [Fact]
        public void Run_EnumerateDirectories_Basic_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"]
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = globber.EnumerateDirectories();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedDirectories.Length, result.Count());
        }

        [Fact]
        public void Run_EnumerateFiles_Basic_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"]
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = globber.EnumerateFiles();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedFiles.Length, result.Count());
        }

        [Fact]
        public async Task Run_EnumerateDirectoriesAsync_Basic_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"]
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = await globber.EnumerateDirectoriesAsync();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedDirectories.Length, result.Count());
        }

        [Fact]
        public async Task Run_EnumerateFilesAsync_Basic_Full()
        {
            _windowsTDHandler ??= new TestDataHandler(TestDataKind.WINDOWS);
            _linuxTDHandler ??= new TestDataHandler(TestDataKind.LINUX);

            // Prepare
            var testOptions = new GlobOptions()
            {
                RootPath = _windowsTDHandler.ExpectedData.RootPath,
                MatchPatterns = ["*"]
            };

            var globber = new Globber(testOptions);

            // Execute
            var result = await globber.EnumerateFilesAsync();

            // Validate
            Assert.NotNull(result);
            Assert.Equal(_windowsTDHandler.ExpectedData.ExpectedFiles.Length, result.Count());
        }

        ~BasicUsageTests()
        {
            _windowsTDHandler?.Dispose();
            _linuxTDHandler?.Dispose();
        }
    }
}