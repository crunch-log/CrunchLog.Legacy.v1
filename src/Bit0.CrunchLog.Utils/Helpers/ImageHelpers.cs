using Bit0.CrunchLog.Extensions;
using System;
using System.IO;
using System.Linq;

namespace Bit0.CrunchLog.Helpers
{
    public static class ImageHelpers
    {
        public static FileInfo GetImagePath(String fileKey, DirectoryInfo basePath, DirectoryInfo imagesPath, FileInfo fallback)
        {
            var path = basePath.CombineFilePath(fileKey);
            if (!path.Exists)
            {
                path = imagesPath.CombineFilePath(fileKey);
            }

            if (!path.Exists)
            {
                path = imagesPath.GetFiles(fileKey).FirstOrDefault();
            }

            if (path != null && path.Exists)
            {
                return path;
            }

            return fallback;
        }
    }
}
