﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void ValidatePatterns()
        {
            /// VALID | No required patterns
            if (MatchPatterns.Count == 0)
            { throw new ArgumentException("No match patterns have been specified."); }

            /// TRIM | Duplicates
            MatchPatterns = MatchPatterns.Distinct().ToList();
            ExcludePatterns = ExcludePatterns.Distinct().ToList();

            /// VALID | Complete overlap
            if (MatchPatterns.Intersect(ExcludePatterns).Count() == MatchPatterns.Count)
            { throw new ArgumentException("Match and exclude patterns cannot completely overlap."); }

            /// TRIM | Cancel-out overlap
            var intersect = MatchPatterns.Intersect(ExcludePatterns);
            if (intersect.Any())
            {
                MatchPatterns = MatchPatterns.Except(intersect).ToList();
                ExcludePatterns = ExcludePatterns.Except(intersect).ToList();
            }
        }
    }
}