using FileGobbler.TestUtilities.Utilities;

namespace FileGobbler.TestUtilities.Services
{
    public class TestDataHandler : IDisposable
    {
        public class TestDataExpected
        {
            public string RootPath { get; init; } = string.Empty;
            public TestDataKind TestDataType { get; init; }
            public string[] ExpectedFiles { get; init; } = [];
            public string[] ExpectedDirectories { get; init; } = [];
        }

        public enum TestDataKind
        {
            Linux,
            Windows
        }

        private static readonly object _sync = new();
        private static readonly Dictionary<TestDataKind, int> _refCounts = new();
        private static readonly Dictionary<TestDataKind, string> _extractDirs = new();

        private static readonly string TEST_ROOT_DIR = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        private static readonly string TEST_LINUX_ARCHIVE_PATH = Path.Combine(TEST_ROOT_DIR, typeof(TestDataHandler).Assembly.GetName().Name!, "test-data", "Linux", "LinuxTestArchive.zip");
        private static readonly string TEST_WINDOWS_ARCHIVE_PATH = Path.Combine(TEST_ROOT_DIR, typeof(TestDataHandler).Assembly.GetName().Name!, "test-data", "Windows", "WindowsTestArchive.zip");
        private const string TEST_DATA_EXTRACT_PREFIX = "Handled_Test_Data";
        private readonly TestDataKind _kind;
        public TestDataExpected Data { get; }

        public TestDataHandler(TestDataKind kind)
        {
            _kind = kind;
            lock (_sync)
            {
                if (!_refCounts.TryGetValue(kind, out var count) ||
                    (count == 0))
                {
                    var zipPath = kind == TestDataKind.Linux
                        ? TEST_LINUX_ARCHIVE_PATH
                        : TEST_WINDOWS_ARCHIVE_PATH;

                    var extractPrefix = $"{TEST_DATA_EXTRACT_PREFIX}_{kind}";

                    var extractDir = ZipExtractHelper.ExtractToTempDirectory(zipPath, extractPrefix);

                    _extractDirs[kind] = extractDir;
                    _refCounts[kind] = 1;
                }
                else
                { _refCounts[kind] = count + 1; }

                var root = _extractDirs[kind];
                Data = new TestDataExpected
                {
                    RootPath = root,
                    TestDataType = kind,
                    ExpectedFiles = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories).ToArray(),
                    ExpectedDirectories = Directory.EnumerateDirectories(root, "*", SearchOption.AllDirectories).ToArray()
                };
            }
        }

        public void Dispose()
        {
            lock (_sync)
            {
                var count = _refCounts[_kind] - 1;
                if (count <= 0)
                {
                    var dir = _extractDirs[_kind];
                    if (Directory.Exists(dir))
                    { Directory.Delete(dir, recursive: true); }

                    _refCounts.Remove(_kind);
                    _extractDirs.Remove(_kind);
                }
                else
                { _refCounts[_kind] = count; }
            }
        }
    }
}