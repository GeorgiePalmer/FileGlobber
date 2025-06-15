using GP.FileGlobber.Extensions;
using GP.FileGlobber.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GP.FileGlobber.Services
{
    public class GlobBase
    {
        protected readonly GlobOptions _options;

        public GlobBase(GlobOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Recursively retrieves a list of directory paths relative to a specified root path, up to a maximum depth.
        /// </summary>
        /// <remarks>The method skips directories that are shorter than or equal to the configured prefix
        /// length, which typically represents the root path itself. The search is limited by the maximum depth
        /// specified in the options.</remarks>
        /// <param name="path">The root directory path from which to begin the search.</param>
        /// <param name="depth">The current depth of the search. This value is incremented with each recursive call.</param>
        /// <returns>A list of directory paths relative to the root path. Returns an empty list if the maximum depth is reached
        /// or no directories are found.</returns>
        protected List<string> GetDirectories(string path, uint depth)
        {
            if (depth >= _options.MaxDepth)
            { return []; } // Stop when the depth limit is reached

            List<string> directories = [];

            foreach (var fullDir in Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly))
            {
                if (fullDir.Length <= _options.PrefixLength)
                { continue; } // Skip if the directory is the root path itself

                directories.Add(fullDir.Substring(_options.PrefixLength));

                foreach (var subDir in GetDirectories(fullDir, depth + 1))
                { // Recursively get subdirectories
                    directories.Add(subDir);
                }
            }

            return directories;
        }

        /// <summary>
        /// Recursively retrieves a list of file paths from the specified directory, up to a maximum depth.
        /// </summary>
        /// <remarks>This method retrieves file paths relative to the root directory by removing a
        /// configurable prefix length from each path. Subdirectories are processed recursively, but recursion stops
        /// when the specified maximum depth is reached.</remarks>
        /// <param name="path">The root directory from which to start retrieving file paths. Must be a valid directory path.</param>
        /// <param name="depth">The current recursion depth. This value is used internally to enforce the maximum depth limit.</param>
        /// <returns>A list of file paths relative to the root directory, excluding the root directory itself. Returns an empty
        /// list if the maximum depth is reached or if no files are found.</returns>
        protected List<string> GetFiles(string path, uint depth)
        {
            if (depth >= _options.MaxDepth)
            { return []; } // Stop when the depth limit is reached

            List<string> files = [];

            foreach (var fullFile in Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly))
            {
                if (fullFile.Length <= _options.PrefixLength)
                { continue; } // Skip if the file is the root path itself

                files.Add(fullFile.Substring(_options.PrefixLength));
            }

            foreach (var fullDir in Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly))
            { // Recursively get subdirectories
                if (fullDir.Length <= _options.PrefixLength)
                { continue; } // Skip if the directory is the root path itself

                foreach (var subFile in GetFiles(fullDir, depth + 1))
                { // Recursively get subfiles
                    files.Add(subFile);
                }
            }

            return files;
        }

        /// <summary>
        /// Asynchronously retrieves a list of relative directory paths starting from the specified root path,  up to a
        /// specified depth.
        /// </summary>
        /// <remarks>This method recursively enumerates directories up to the maximum depth specified in
        /// the options.  Directories at or beyond the maximum depth are not included in the result. The method skips
        /// the  root directory itself and only includes subdirectories.</remarks>
        /// <param name="path">The root directory path from which to begin the search.</param>
        /// <param name="depth">The current depth of the search. This value is incremented recursively and should typically  be set to 0
        /// when calling this method initially.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of relative  directory
        /// paths, relative to the root path, found within the specified depth.</returns>
        protected async Task<List<string>> GetDirectoriesAsync(string path, uint depth)
        {
            if (depth >= _options.MaxDepth)
            { return []; } // Stop when the depth limit is reached

            var entries = await Task.Run(() =>
            {
                return Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly).ToList();
            });

            List<string> directories = [];
            foreach (var fullDir in entries)
            {
                if (fullDir.Length <= _options.PrefixLength)
                { continue; } // Skip if the directory is the root path itself

                directories.Add(fullDir.Substring(_options.PrefixLength));

                var sub = await GetDirectoriesAsync(fullDir, depth + 1); // Recursively get subdirectories
                directories.AddRange(sub);
            }

            return directories;
        }

        /// <summary>
        /// Asynchronously retrieves a list of file paths relative to a specified root directory,  traversing
        /// subdirectories up to a specified depth.
        /// </summary>
        /// <remarks>This method recursively traverses subdirectories starting from the specified root
        /// directory,  collecting file paths relative to the root. The traversal stops when the maximum depth,  as
        /// defined by the configuration options, is reached. Files and directories that match the  root path itself are
        /// skipped.</remarks>
        /// <param name="path">The root directory path from which to start the file search.</param>
        /// <param name="depth">The current depth of the directory traversal. This value is incremented recursively  and should typically be
        /// set to 0 when calling this method initially.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  file paths relative
        /// to the root directory. The list will be empty if no files are found  or if the maximum depth is reached.</returns>
        protected async Task<List<string>> GetFilesAsync(string path, uint depth)
        {
            if (depth >= _options.MaxDepth)
            { return []; } // Stop when the depth limit is reached

            var files = await Task.Run(() =>
            {
                return Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly).ToList();
            });

            var directories = new List<string>();
            foreach (var fullFile in files)
            {
                if (fullFile.Length <= _options.PrefixLength)
                { continue; } // Skip if the file is the root path itself

                directories.Add(fullFile.Substring(_options.PrefixLength));
            }

            var subdirs = await Task.Run(() =>
            {
                return Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly).ToList();
            });

            foreach (var dir in subdirs)
            { // Recursively get subdirectories
                if (dir.Length <= _options.PrefixLength)
                { continue; } // Skip if the directory is the root path itself

                var subfiles = await GetFilesAsync(dir, depth + 1); // Recursively get subfiles
                directories.AddRange(subfiles);
            }

            return directories;
        }

        /// <summary>
        /// Filters a collection of file paths based on match and exclude patterns.
        /// </summary>
        /// <remarks>Match and exclude patterns are defined in the options and are converted to regular
        /// expressions.  The filtering is case-sensitive or case-insensitive based on the configuration.</remarks>
        /// <param name="paths">A list of file paths to be filtered.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the file paths that match at least one of the  specified match
        /// patterns and do not match any of the specified exclude patterns.</returns>
        protected IEnumerable<string> FilterPathPatterns(List<string> paths)
        {
            /// Pre-validate
            _options.ValidatePatterns();
            var isWindowsPath = DetectIsWindowsPath(paths);

            /// Collect the match and exclude rules
            var matchRules = _options.MatchPatterns
                .Select(mPat => mPat.ToPatternRegex(isWindowsPath, _options.IgnoreCase))
                .ToArray();
            var excludeRules = _options.ExcludePatterns
                .Select(ePat => ePat.ToPatternRegex(isWindowsPath, _options.IgnoreCase))
                .ToArray();

            /// Apply the rules
            return paths.Where(file =>
                matchRules.Any(mRule => mRule.IsMatch(file)) &&
                !excludeRules.Any(eRule => eRule.IsMatch(file)));
        }

        /// <summary>
        /// Determines whether the provided list of paths predominantly uses Windows-style directory separators.
        /// </summary>
        /// <remarks>This method counts the occurrences of backslashes ('\') and forward slashes ('/') in
        /// the provided paths. A positive score indicates a predominance of Windows-style paths, while a negative or
        /// zero score indicates Unix-style paths.</remarks>
        /// <param name="paths">A list of file or directory paths to analyze.</param>
        /// <returns><see langword="true"/> if the majority of directory separators in the provided paths are backslashes ('\'),
        /// indicating Windows-style paths; otherwise, <see langword="false"/>.</returns>
        protected static bool DetectIsWindowsPath(List<string> paths)
        {
            /// Detect if paths provided are Windows or Unix dir-separated
            int score = 0;
            foreach (var path in paths)
            { // Count the number of backslashes and forward slashes in the paths
                score += path.Count(c => c == '\\');
                score -= path.Count(c => c == '/');
            }
            return score > 0;
        }
    }
}