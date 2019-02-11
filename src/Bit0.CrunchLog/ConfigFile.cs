using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using System.IO;
using System.Linq;

namespace Bit0.CrunchLog
{
    public class ConfigFile
    {
        public FileInfo File { get; }

        public ConfigFile(Arguments arguments)
        {
            var basePath = new DirectoryInfo(arguments.BasePath);
            var configFile = basePath.GetFiles(StaticPaths.ConfigFile, SearchOption.TopDirectoryOnly).SingleOrDefault();

            if (configFile == null)
            {
                var errorMsg = $"Cannot find {basePath.CombineDirPath(StaticPaths.ConfigFile)}";
                throw new FileNotFoundException(errorMsg);
            }

            File = configFile;
        }
    }
}
