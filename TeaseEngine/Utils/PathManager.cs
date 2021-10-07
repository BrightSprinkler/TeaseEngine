using System;
using System.IO;

namespace TeaseEngine.Utils
{
    public class PathManager
    {
        public string RootDirectory => AppDomain.CurrentDomain.BaseDirectory;
        public string EscapedRootDirectory => AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", @"\\").TrimEnd('\\');
        public string ModuleDirectory => CreateDirectoryIfNotExists(Path.Combine(RootDirectory, "Modules"));
        public string StorageDirectory => CreateDirectoryIfNotExists(Path.Combine(RootDirectory, "Storage"));
        public string MediaDirectory => CreateDirectoryIfNotExists(Path.Combine(RootDirectory, "Media"));
        public string LogDirectory => CreateDirectoryIfNotExists(Path.Combine(RootDirectory, "Log"));

        public string CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
    }
}
