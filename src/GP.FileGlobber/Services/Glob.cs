using GP.FileGlobber.Models;

namespace GP.FileGlobber.Services
{
    public static class Glob
    {
        public static Globber Create(string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
            { throw new ArgumentException("Root path cannot be null or empty.", nameof(rootPath)); }
            if (!Path.IsPathFullyQualified(rootPath))
            { throw new ArgumentException("Root path must be a fully qualified path.", nameof(rootPath)); }

            var options = new GlobOptions()
            {
                RootPath = rootPath
            };

            return new Globber(options);
        }
    }
}