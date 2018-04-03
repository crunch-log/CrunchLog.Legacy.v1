using System;
using System.Collections.Generic;
using System.IO;
using Markdig.Extensions.DefinitionLists;

namespace Bit0.CrunchLog.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static String CombinePath(this DirectoryInfo dir, params String[] paths)
        {
            var pathList = new List<String>(new[] { dir.FullName });
            pathList.AddRange(paths);

            return Path.Combine(pathList.ToArray());
        }

        public static String CombinePathEx(this DirectoryInfo dir, String extension, params String[] paths)
        {
            var pathList = new List<String>(new[] { dir.FullName });
            pathList.AddRange(paths);

            return Path.ChangeExtension(dir.CombinePath(pathList.ToArray()), extension);
        }
        
        public static String NormalizePath(this String path)
        {
            return path.Replace('\\', '/').Replace('/', Path.DirectorySeparatorChar);
        }

        
        public static void Copy(this DirectoryInfo dir, DirectoryInfo destDir, Boolean copySubDirs = true)
        {
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + dir.FullName);
            }

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!destDir.Exists)
            {
                destDir.Create();
            }
        
            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = destDir.CombinePath(file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    var temppath = new DirectoryInfo(destDir.CombinePath(subdir.Name));
                    subdir.Copy(temppath, copySubDirs);
                }
            }
        }
    }
}
