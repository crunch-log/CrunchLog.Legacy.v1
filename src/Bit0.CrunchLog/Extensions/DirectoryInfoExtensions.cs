using System;
using System.Collections.Generic;
using System.IO;

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
    }
}
