using System;
using System.IO;

namespace BitmapCompressor.Tests.Helpers
{
    public static class TestResourceDirectory
    {
        public static readonly string ProjectDirectory;
        public static readonly string ResourceDirectory;

        static TestResourceDirectory()
        {
            var baseDirectoryInfo = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);

            ProjectDirectory = baseDirectoryInfo?.Parent?.Parent?.FullName;
            ResourceDirectory = Path.Combine(ProjectDirectory, "Resources");

            if (!Directory.Exists(ResourceDirectory))
            {
                throw new DirectoryNotFoundException($"Test resource directory not found: {ResourceDirectory}");
            }
        }

        public static string GetFilePath(string resourceFile)
        {
            var path = Path.Combine(ResourceDirectory, resourceFile);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Test resource file not found: {path}");
            }

            return path;
        }
    }
}
