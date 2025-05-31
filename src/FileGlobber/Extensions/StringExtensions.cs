using System;
using System.Text.RegularExpressions;

namespace FileGlobber.Extensions
{
    public static class StringExtensions
    {
        internal static Regex ToPatternRegex(this string pattern, bool isWindowsOS, bool ignoreCase = false)
        {
            /// Takes a single string, which can be either:
            ///   • a literal regex (e.g. "([A-Z]{1,2}-[0-9]+_?)+/.*")
            ///   • a classic CLI glob (e.g. "*", "**/*", "*.png", "foo?bar")
            if (TryMakeRegex(pattern, ignoreCase, out Regex? regex))
            {
                if (regex == null)
                { throw new ArgumentException("Invalid regex"); }
                return regex;
            }
            else
            {
                // Otherwise, treat “pattern” as a CLI‐style glob and convert it.
                string regexFromGlob = GlobToRegex(pattern, isWindowsOS);

                if (!TryMakeRegex(regexFromGlob, ignoreCase, out regex) ||
                    (regex == null))
                { throw new ArgumentException("Invalid glob pattern"); }

                return regex;
            }
        }

        private static bool TryMakeRegex(string input, bool ignoreCase, out Regex? regex)
        {
            try
            {
                regex = ignoreCase ?
                    new Regex(input, Constants.DEFAULT_REGEX_OPTIONS | RegexOptions.IgnoreCase) :
                    new Regex(input, Constants.DEFAULT_REGEX_OPTIONS);
                return true;
            }
            catch (ArgumentException)
            { regex = null; return false; } // Invalid regex -- Maybe CLI pattern?
        }

        private static string GlobToRegex(string glob, bool sepIsBackslash = false)
        {
            string sepLiteral, sepEscapedInPattern;
            if (sepIsBackslash)
            {
                sepLiteral = @"\";
                sepEscapedInPattern = @"\\\\";
            }
            else
            {
                sepLiteral = "/";
                sepEscapedInPattern = "\\/";
            }

            string escaped = Regex.Escape(glob);
            escaped = escaped.Replace(Regex.Escape(sepLiteral), sepEscapedInPattern);
            escaped = escaped.Replace("\\*", ".*");
            escaped = escaped.Replace("\\?", ".");
            escaped = escaped.Replace("\\", @"[\\/]");
            return "^" + escaped + "$";
        }
    }
}