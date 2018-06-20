using System;
using System.IO;

namespace Bit0.CrunchLog.Extensions
{
    public static class FileInfoExtensions
    {
        public static String GetText(this FileInfo fileInfo)
        {
            using (var sr = fileInfo.OpenText())
            {
                return sr.ReadToEnd();
            }
        }

        public static String ToRelative(this FileInfo fileInfo, DirectoryInfo basePath)
        {
            return fileInfo.FullName.Replace(basePath.FullName, "").NormalizeRelativePath('/');
        }
    }
}
