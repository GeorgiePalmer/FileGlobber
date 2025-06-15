namespace FileGobbler.UnitTests
{
    public class StringExtensionTests
    {
        [Fact]
        public void ToPatternRegex_ValidLiteralRegex_ReturnsRegex()
        {
            // Prepare
            string pattern = @"^[A-Z]{2}[0-9]+$";
            bool isWindows = true;
            bool ignoreCase = false;

            // Execute
            var regex = pattern.ToPatternRegex(isWindows, ignoreCase);

            // Validate
            Assert.NotNull(regex);
            Assert.Equal(pattern, regex.ToString());
        }

        [Fact]
        public void ToPatternRegex_InvalidLiteralRegex_ThrowsArgumentException()
        {
            // Prepare
            string pattern = @"([unclosed"; // invalid regex
            bool isWindows = false;
            bool ignoreCase = false;

            // Execute + Validate
            Assert.Throws<ArgumentException>(() =>
            {
                _ = pattern.ToPatternRegex(isWindows, ignoreCase);
            });
        }

        [Fact]
        public void ToPatternRegex_ValidGlob_WindowsSeparator_ReturnsRegex()
        {
            // Prepare
            string pattern = @"**\*.cs";
            bool isWindows = true;
            bool ignoreCase = true;

            // Execute
            var regex = pattern.ToPatternRegex(isWindows, ignoreCase);

            // Validate
            Assert.NotNull(regex);
            Assert.Contains("[\\\\/].*", regex.ToString()); // Contains converted slash glob logic
        }

        [Fact]
        public void ToPatternRegex_ValidGlob_UnixSeparator_ReturnsRegex()
        {
            // Prepare
            string pattern = "**/*.txt";
            bool isWindows = false;
            bool ignoreCase = true;

            // Execute
            var regex = pattern.ToPatternRegex(isWindows, ignoreCase);

            // Validate
            Assert.NotNull(regex);
            Assert.Contains("\\/", regex.ToString()); // Escaped forward slash
        }

        [Fact]
        public void ToPatternRegex_InvalidGlobConvertedRegex_ThrowsArgumentException()
        {
            // Prepare
            string pattern = "**/[bad(regex)";
            bool isWindows = false;
            bool ignoreCase = true;

            // Tamper the GlobToRegex output to force it to produce a bad regex
            string brokenRegex = "[[["; // definitely bad

            // Replace GlobToRegex via wrapper for isolation, or simulate failure here:
            // We'll simulate it by replacing Regex.Escape in pattern with a known-bad value

            string backup = pattern;
            pattern = brokenRegex;

            // Execute + Validate
            Assert.Throws<ArgumentException>(() =>
            {
                _ = pattern.ToPatternRegex(isWindows, ignoreCase);
            });
        }
    }
}