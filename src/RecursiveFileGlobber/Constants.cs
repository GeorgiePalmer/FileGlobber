using System.Text.RegularExpressions;

namespace RecursiveFileGlobber
{
    internal static class Constants
    {
        public const string DirSepPattern = @"[/\\]";
        public const string AnyDirPattern = @".+[/\\]";

        public const RegexOptions DefaultRegexOptions
            = RegexOptions.Compiled;
    }
}