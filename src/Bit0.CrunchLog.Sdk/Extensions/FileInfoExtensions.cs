using Newtonsoft.Json;
using System;
using System.IO;

namespace Bit0.CrunchLog.Extensions
{
    public static class FileInfoExtensions
    {
        public static String ReadText(this FileInfo fileInfo)
        {
            using (var sr = fileInfo.OpenText())
            {
                return sr.ReadToEnd();
            }
        }

        public static void WriteText(this FileInfo fileInfo, String text)
        {
            using (var sw = new StreamWriter(fileInfo.FullName, append: false))
            {
                sw.Write(text);
            }
        } 

        public static String ToRelative(this FileInfo fileInfo, DirectoryInfo basePath)
        {
            return fileInfo.FullName.Replace(basePath.FullName, "").NormalizeRelativePath('/');
        }

        public static JsonTextReader OpenJsonReader(this FileInfo entry)
        {
            return entry.OpenRead().GetJsonReader();
        }

        public static JsonTextReader GetJsonReader(this Stream stream)
        {
            return new JsonTextReader(new StreamReader(stream));
        }
    }
}
