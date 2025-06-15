using GP.FileGlobber.Models;
using GP.FileGlobber.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GP.FileGlobber.Services
{
    public class Globber : GlobBase, IGlobber
    {
        public GlobOptions Options => _options;
        private const int ROOT_DEPTH = 0;

        public Globber(GlobOptions options) : base(options)
        { }

        /// <summary>
        /// Configures the globbing operation to perform case-insensitive matching.
        /// </summary>
        /// <remarks>When this method is called, all subsequent glob patterns will be matched  without
        /// regard to case sensitivity. This is useful for file systems or  environments where case sensitivity is not
        /// enforced.</remarks>
        /// <returns>The current <see cref="Globber"/> instance, allowing for method chaining.</returns>
        public IGlobber CaseInsensitive()
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
        public IGlobber Exclude(string excludePattern)
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
        public IGlobber IncludeHidden()
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
        public IGlobber Match(string matchPattern)
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
        public IGlobber MaxDepth(uint maxDepth)
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
            var allDirectories = GetDirectories(_options.RootPath, ROOT_DEPTH);

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
            var allFiles = GetFiles(_options.RootPath, ROOT_DEPTH);

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
            var allDirectories = await GetDirectoriesAsync(_options.RootPath, ROOT_DEPTH);

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
            var allFiles = await GetFilesAsync(_options.RootPath, ROOT_DEPTH);

            /// Apply filters
            var filteredFiles = FilterPathPatterns(allFiles);
            return filteredFiles;
        }
    }
}