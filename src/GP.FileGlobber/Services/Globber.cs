using GP.FileGlobber.Extensions;
using GP.FileGlobber.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GP.FileGlobber.Services
{
    public class Globber
    {
        private readonly GlobOptions _options;
        public GlobOptions Options => _options;

        public Globber(GlobOptions options)
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
        private List<string> GetDirectories(string path, uint depth)
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
        private List<string> GetFiles(string path, uint depth)
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
        private async Task<List<string>> GetDirectoriesAsync(string path, uint depth)
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
        private async Task<List<string>> GetFilesAsync(string path, uint depth)
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
        private IEnumerable<string> FilterPathPatterns(List<string> paths)
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

        private static bool DetectIsWindowsPath(List<string> paths)
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

        /// <summary>
        /// Configures the globbing operation to perform case-insensitive matching.
        /// </summary>
        /// <remarks>When this method is called, all subsequent glob patterns will be matched  without
        /// regard to case sensitivity. This is useful for file systems or  environments where case sensitivity is not
        /// enforced.</remarks>
        /// <returns>The current <see cref="Globber"/> instance, allowing for method chaining.</returns>
        public Globber CaseInsensitive()
        {
            _options.IgnoreCase = true;
            return this;
        }

        /// <summary>
        /// Adds a pattern to the list of excluded paths for the globbing operation.
        /// </summary>
        /// <remarks>Excluded patterns are applied in addition to any included patterns, ensuring that
        /// matching paths are ignored. Patterns should follow standard globbing syntax.</remarks>
        /// <param name="excludePattern">The glob pattern to exclude. This pattern determines which paths will be ignored during the globbing
        /// process.</param>
        /// <returns>The current <see cref="Globber"/> instance, allowing for method chaining.</returns>
        public Globber Exclude(string excludePattern)
        {
            _options.ExcludePatterns.Add(excludePattern);
            return this;
        }

        /// <summary>
        /// Configures the globbing operation to include hidden files and directories in the results.
        /// </summary>
        /// <remarks>By default, hidden files and directories are excluded from the globbing operation.
        /// Calling this method modifies the behavior to include them. Hidden files and directories  are typically those
        /// with names starting with a dot (e.g., ".hiddenfile").</remarks>
        /// <returns>The current <see cref="Globber"/> instance with the updated configuration, allowing for method chaining.</returns>
        public Globber IncludeHidden()
        {
            _options.IncludeHidden = true;
            return this;
        }

        /// <summary>
        /// Adds a match pattern to the current globbing configuration.
        /// </summary>
        /// <remarks>The added match pattern will be used to identify files or directories that match the
        /// specified criteria. Patterns are evaluated based on the globbing rules defined in the <see cref="Globber"/>
        /// configuration.</remarks>
        /// <param name="matchPattern">The pattern to match against file or directory paths. This can include wildcard characters (e.g., <c>*</c> or
        /// <c>?</c>) to define flexible matching rules.</param>
        /// <returns>The current <see cref="Globber"/> instance, allowing for method chaining.</returns>
        public Globber Match(string matchPattern)
        {
            _options.MatchPatterns.Add(matchPattern);
            return this;
        }

        /// <summary>
        /// Sets the maximum depth for directory traversal when matching file patterns.
        /// </summary>
        /// <param name="maxDepth">The maximum depth, as a non-negative integer, to traverse directories. A value of 0 limits traversal to the
        /// current directory only.</param>
        /// <returns>The current <see cref="Globber"/> instance, allowing for method chaining.</returns>
        public Globber MaxDepth(uint maxDepth)
        {
            _options.MaxDepth = maxDepth;
            return this;
        }

        /// <summary>
        /// Enumerates the directories within the specified root path.
        /// </summary>
        /// <remarks>This method sets the root path for enumeration and returns the directories found
        /// within it.  The enumeration may include subdirectories depending on the implementation of the underlying
        /// enumeration logic.</remarks>
        /// <param name="rootPath">The root path from which to begin directory enumeration. Must be a valid, non-null, and non-empty string.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of strings representing the paths of the directories found within the
        /// specified root path.</returns>
        public IEnumerable<string> EnumerateDirectories(string rootPath)
        {
            _options.RootPath = rootPath;
            return EnumerateDirectories();
        }

        /// <summary>
        /// Enumerates all file paths within the specified root directory.
        /// </summary>
        /// <remarks>This method sets the root directory for file enumeration and returns the results.
        /// Subsequent calls to this method will use the updated root directory.</remarks>
        /// <param name="rootPath">The root directory path from which to begin file enumeration. Cannot be null or empty.</param>
        /// <returns>An enumerable collection of file paths found within the specified root directory.</returns>
        public IEnumerable<string> EnumerateFiles(string rootPath)
        {
            _options.RootPath = rootPath;
            return EnumerateFiles();
        }

        /// <summary>
        /// Asynchronously retrieves a collection of directory paths starting from the specified root path.
        /// </summary>
        /// <remarks>This method updates the root path for the enumeration operation and retrieves the
        /// directories asynchronously.</remarks>
        /// <param name="rootPath">The root directory path from which to begin enumeration. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// directory paths.</returns>
        public Task<IEnumerable<string>> EnumerateDirectoriesAsync(string rootPath)
        {
            _options.RootPath = rootPath;
            return EnumerateDirectoriesAsync();
        }

        /// <summary>
        /// Asynchronously enumerates all file paths within the specified root directory.
        /// </summary>
        /// <remarks>This method sets the root directory for the file enumeration operation and initiates
        /// the asynchronous process. The enumeration includes all files within the specified directory and its
        /// subdirectories.</remarks>
        /// <param name="rootPath">The root directory path to search for files. Must be a valid, non-null, and non-empty string.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/>
        /// of strings,  where each string is the full path of a file found within the specified root directory.</returns>
        public Task<IEnumerable<string>> EnumerateFilesAsync(string rootPath)
        {
            _options.RootPath = rootPath;
            return EnumerateFilesAsync();
        }

        /// <summary>
        /// Enumerates all directories within the root path, applying any configured filters.
        /// </summary>
        /// <remarks>This method retrieves all directories starting from the root path specified in the
        /// options and filters them based on the configured path patterns. The enumeration is performed lazily, meaning
        /// directories are processed as they are iterated.</remarks>
        /// <returns>An <see cref="IEnumerable{T}"/> of strings representing the filtered directory paths.</returns>
        public IEnumerable<string> EnumerateDirectories()
        {
            /// Get all directories in the root path
            uint depth = 0;
            var allDirectories = GetDirectories(_options.RootPath, depth);

            /// Apply filters
            var filteredDirectories = FilterPathPatterns(allDirectories);
            return filteredDirectories;
        }

        /// <summary>
        /// Enumerates all files in the root path, applying any configured filters.
        /// </summary>
        /// <remarks>This method retrieves all files from the root path specified in the options and
        /// filters them  based on the configured path patterns. The returned collection includes only the files that
        /// match the filter criteria.</remarks>
        /// <returns>An <see cref="IEnumerable{T}"/> of strings representing the paths of the filtered files.</returns>
        public IEnumerable<string> EnumerateFiles()
        {
            /// Get all files in the root path
            uint depth = 0;
            var allFiles = GetFiles(_options.RootPath, depth);

            /// Apply filters
            var filteredFiles = FilterPathPatterns(allFiles);
            return filteredFiles;
        }

        /// <summary>
        /// Asynchronously enumerates all directories in the root path, applying any configured filters.
        /// </summary>
        /// <remarks>This method retrieves all directories starting from the root path specified in the
        /// options and applies any path filtering patterns defined in the configuration. The returned collection
        /// contains only the directories that match the applied filters.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// directory paths that match the applied filters.</returns>
        public async Task<IEnumerable<string>> EnumerateDirectoriesAsync()
        {
            /// Get all directories in the root path
            uint depth = 0;
            var allDirectories = await GetDirectoriesAsync(_options.RootPath, depth);

            /// Apply filters
            var filteredDirectories = FilterPathPatterns(allDirectories);
            return filteredDirectories;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of file paths from the root directory,  applying any configured
        /// filters to the results.
        /// </summary>
        /// <remarks>This method starts at the root directory specified in the options and retrieves  all
        /// files, applying any path pattern filters defined in the configuration.  The returned collection contains
        /// only the files that match the specified filters.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains  an <see cref="IEnumerable{T}"/>
        /// of strings, where each string is the path of a file  that matches the configured filters.</returns>
        public async Task<IEnumerable<string>> EnumerateFilesAsync()
        {
            /// Get all files in the root path
            uint depth = 0;
            var allFiles = await GetFilesAsync(_options.RootPath, depth);

            /// Apply filters
            var filteredFiles = FilterPathPatterns(allFiles);
            return filteredFiles;
        }
    }
}