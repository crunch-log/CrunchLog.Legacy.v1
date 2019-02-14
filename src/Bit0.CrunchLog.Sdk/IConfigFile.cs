using System.IO;
using Bit0.CrunchLog.Config;
using Newtonsoft.Json.Linq;

namespace Bit0.CrunchLog
{
    public interface IConfigFile
    {
        DirectoryInfo Directory { get; }
        FileInfo File { get; }
        JObject JsonObject { get; }
        ConfigPaths Paths { get; }
    }
}