using GP.TestUtilities.Enums;

namespace GP.TestUtilities.Models
{
    public class TestDataExpected
    {
        public string RootPath { get; init; } = string.Empty;
        public TestDataKind TestDataType { get; init; }
        public string[] ExpectedFiles { get; init; } = [];
        public string[] ExpectedDirectories { get; init; } = [];
    }
}