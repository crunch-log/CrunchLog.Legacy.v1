using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.TemplateModels;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Theme
{
    public abstract class ThemeHandlerBase : IThemeHandler
    {
        private readonly JsonSerializer _jsonSerializer;

        protected ThemeHandlerBase(CrunchConfig config, JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
            Config = config;
        }

        protected CrunchConfig Config { get; }

        public void InitOutput()
        {
            var themeConfig = new FileInfo(Config.Site.Theme.CombinePath("theme.json"));
            var theme = new Theme(themeConfig);
            _jsonSerializer.Populate(themeConfig.OpenText(), theme);

            if (!Config.Paths.OutputPath.Exists)
            {
                Config.Paths.OutputPath.Create();
            }

            foreach (var dir in theme.Assets.Directories.Select(d => new DirectoryInfo(theme.Directory.CombinePath(d))))
            {
                dir.Copy(new DirectoryInfo(Config.Paths.OutputPath.CombinePath(dir.Name)));
            }

            foreach (var file in theme.Assets.Files.Select(f => new FileInfo(theme.Directory.CombinePath(f))))
            {
                file.CopyTo(Config.Paths.OutputPath.CombinePath(file.Name), true);
            }
        }

        public abstract void WriteFile(String template, ITemplateModel model);
    }
}
