using System.Collections.Generic;
using System.Threading.Tasks;

namespace GP.FileGlobber
{
    public interface IGlobber
    {
        /// <summary>
        /// Enumerates the names of all directories within the specified root path.
        /// </summary>
        /// <param name="rootPath">The root directory path to search for subdirectories. Must be a valid, non-null, and non-empty string.</param>
        /// <returns>An enumerable collection of directory names found within the specified root path. If no directories are
        /// found, the collection will be empty.</returns>
        IEnumerable<string> EnumerateDirectories(string rootPath);

        /// <summary>
        /// Enumerates all file paths within the specified root directory and its subdirectories.
        /// </summary>
        /// <remarks>The method performs a recursive search, including all subdirectories of the specified
        /// root directory. Hidden and system files are included in the results. The enumeration is deferred, meaning
        /// files are retrieved lazily as the collection is iterated.</remarks>
        /// <param name="rootPath">The root directory to search for files. Must be a valid, non-null, and non-empty path.</param>
        /// <returns>An enumerable collection of file paths found within the specified directory and its subdirectories.</returns>
        IEnumerable<string> EnumerateFiles(string rootPath);

        /// <summary>
        /// Asynchronously retrieves the names of all directories within the specified root path.
        /// </summary>
        /// <remarks>The method performs a non-blocking, asynchronous operation to enumerate directories.
        /// It does not include files or nested directories within the returned collection.</remarks>
        /// <param name="rootPath">The root directory path to search for subdirectories. Must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// directory names. If no directories are found, the collection will be empty.</returns>
        Task<IEnumerable<string>> EnumerateDirectoriesAsync(string rootPath);

        /// <summary>
        /// Asynchronously enumerates all file paths within the specified root directory and its subdirectories.
        /// </summary>
        /// <remarks>This method performs a recursive search of the directory tree starting at <paramref
        /// name="rootPath"/>.  The enumeration is case-sensitive on case-sensitive file systems. The method does not
        /// guarantee the order of the returned file paths.</remarks>
        /// <param name="rootPath">The root directory path to search for files. Must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{string}"/>, where each string is the full path of a file found within the directory hierarchy.  If no files
        /// are found, the result will be an empty collection.</returns>
        Task<IEnumerable<string>> EnumerateFilesAsync(string rootPath);

        /// <summary>
        /// Enumerates the names of all directories in the current context.
        /// </summary>
        /// <remarks>The method does not guarantee the order of the returned directory names.  Callers should
        /// handle any potential exceptions that may occur during enumeration,  such as access violations or I/O
        /// errors.</remarks>
        /// <returns>An <see cref="IEnumerable{string}"/>, where each string represents the name of a directory. The
        /// collection will be empty if no directories are found.</returns>
        IEnumerable<string> EnumerateDirectories();

        /// <summary>
        /// Enumerates the file paths in the current directory.
        /// </summary>
        /// <returns>An enumerable collection of strings, where each string represents the full path of a file in the directory.
        /// The collection will be empty if no files are found.</returns>
        IEnumerable<string> EnumerateFiles();

        /// <summary>
        /// Asynchronously retrieves the names of all directories in the current context.
        /// </summary>
        /// <remarks>This method does not guarantee the order of the returned directory names.  The caller
        /// is responsible for handling any exceptions that may occur during the enumeration process.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IEnumerable{string}"/>,
        /// where each string is the name of a directory. If no directories are found, the result will be an empty
        /// collection.</returns>
        Task<IEnumerable<string>> EnumerateDirectoriesAsync();

        /// <summary>
        /// Asynchronously retrieves a collection of file paths from the current directory.
        /// </summary>
        /// <remarks>The method returns an enumerable collection of file paths as strings. The collection
        /// may be empty if no files are found. This method does not guarantee the order of the returned file
        /// paths.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{string}"/>, where each string is the path to a file in the current directory.</returns>
        Task<IEnumerable<string>> EnumerateFilesAsync();
    }
}