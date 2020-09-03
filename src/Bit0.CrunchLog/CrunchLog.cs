using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Helpers;
using Bit0.Registry.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace Bit0.CrunchLog
{
    public class CrunchLog
    {
        private readonly JsonSerializer _jsonSerializer;
        private readonly IPackageManager _packageManager;
        private readonly ILogger<CrunchLog> _logger;

        public CrunchConfig SiteConfig { get; }

        public CrunchLog(Arguments args, JsonSerializer jsonSerializer, IPackageManager packageManager, ILogger<CrunchLog> logger)
        {
            _jsonSerializer = jsonSerializer;
            _packageManager = packageManager;
            _logger = logger;
            _logger.LogInformation($"Base path: {args.BasePath}");

            var configFile = new DirectoryInfo(args.BasePath).GetConfigFile(args.LoadConfig);
            SiteConfig = new CrunchConfig(configFile);

            if (args.LoadConfig)
            {
                Load();
            }
        }

        internal void Load()
        {
            ReadConfig();
            LoadCategories();
            LoadTheme();
        }

        private void ReadConfig()
        {
            _logger.LogDebug($"Read configuration: {SiteConfig.File.FullName}");

            var jsonObject = JObject.Parse(SiteConfig.File.ReadText());

            _jsonSerializer.Populate(jsonObject.CreateReader(), SiteConfig);

            _logger.LogDebug("Configuration read");
            _logger.LogInformation($"Output path: {SiteConfig.Paths.OutputPath}");
        }

        private void LoadCategories()
        {
            SiteConfig.Categories = SiteConfig.Categories.ToDictionary(k => k.Key, v =>
            {
                var cat = v.Value;

                cat.Title = v.Key;
                cat.Permalink = String.Format(StaticKeys.CategoryPathFormat, v.Key);

                return cat;
            });
        }

        private void LoadTheme()
        {
            var packFile = SiteConfig.Paths.ThemesPath.CombineDirPath(SiteConfig.Theme.Name).CombineFilePath("pack.json");

            if (!packFile.Exists)
            {
                packFile = _packageManager.GetPack(SiteConfig.Theme.Name).PackFile;
            }

            SiteConfig.Theme = Theme.Get(packFile, SiteConfig.Paths.OutputPath);
        }
    }
}
