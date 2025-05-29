using System.Text.RegularExpressions;

namespace FileGlobber.Extensions
{
    public static class StringExtensions
    {
        internal static Regex ToRegex(this string glob, bool ignoreCase = false)
        { // TODO: Regex is broken, causing depth directories and files to be missed -- "folder1/" and "folder1/folder1-1/" captured but only "folder1" is returned
            /// Escape regex metachars
            var escaped = Regex.Escape(glob);

            /// Handle **/ or /**  → “match any directory segments”
            escaped = escaped
                .Replace(@"\*\*/", $"({Constants.AnyDirPattern})?")
                .Replace(@"/\*\*", $"({Constants.DirSepPattern}.+)?");

            /// Replace single * and ?
            escaped = escaped
                .Replace(@"\*", @"[^/\\]*")
                .Replace(@"\?", @"[^/\\]");

            /// Anchor
            var pattern = $"^{escaped}$";

            return ignoreCase ?
                new Regex(pattern, Constants.DefaultRegexOptions | RegexOptions.IgnoreCase) :
                new Regex(pattern, Constants.DefaultRegexOptions);
        }
    }
}