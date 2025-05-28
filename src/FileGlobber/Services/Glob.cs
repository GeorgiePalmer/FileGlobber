using FileGlobber.Models;
using System;
using System.IO;

namespace FileGlobber.Services
{
    public static class Glob
    {
        public static Globber Create(string rootPath)
        {
            ArgumentNullException.ThrowIfNull(rootPath);
            if (rootPath == string.Empty)
            { throw new ArgumentException("Root path cannot be an empty string.", nameof(rootPath)); }
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