using System;
using System.IO;

namespace Bit0.CrunchLog.Extensions
{
    public static class FileInfoExtensions
    {
        public static String GetText(this FileInfo file)
        {
            using (var sr = file.OpenText())
            {
                return sr.ReadToEnd();
            }
        } 
    }
}
