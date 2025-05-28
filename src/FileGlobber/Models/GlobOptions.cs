using System.Collections.Generic;
using System.IO;

namespace FileGlobber.Models
{
    public class GlobOptions
    {
        public string RootPath { get; set; } = string.Empty;
        public IList<string> MatchPatterns { get; set; } = [];
        public IList<string> ExcludePatterns { get; set; } = [];
        public uint MaxDepth { get; set; } = 50;
        public bool IgnoreCase { get; set; } = false;
        public bool IncludeHidden { get; set; } = false;

        public string NormalizedRoot => RootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        public int PrefixLength => NormalizedRoot.Length;
    }
}