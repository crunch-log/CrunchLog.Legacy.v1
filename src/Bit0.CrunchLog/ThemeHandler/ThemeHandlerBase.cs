using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.TemplateModels;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Bit0.CrunchLog.ThemeHandler
{
    public abstract class ThemeHandlerBase : IThemeHandler
    {
        protected ThemeHandlerBase(CrunchConfig config, JsonSerializer jsonSerializer)
        {
            Config = config;

            var themeConfig = Config.Site.Theme.CombineFilePath(".json", "theme");
            var theme = new Theme(themeConfig);
            jsonSerializer.Populate(themeConfig.OpenText(), theme);

            ThemeConfig = theme;
        }

        protected CrunchConfig Config { get; }
        protected Theme ThemeConfig { get; } 

        public void InitOutput()
        {
            if (!Config.Paths.OutputPath.Exists)
            {
                Config.Paths.OutputPath.Create();
            }

            foreach (var dir in ThemeConfig.Assets.Directories.Select(d => ThemeConfig.Directory.CombineDirPath(d)))
            {
                dir.Copy(Config.Paths.OutputPath.CombineDirPath(dir.Name));
            }

            foreach (var file in ThemeConfig.Assets.Files.Select(f => ThemeConfig.Directory.CombineFilePath(f)))
            {
                file.CopyTo(Config.Paths.OutputPath.CombineDirPath(file.Name).FullName, true);
            }
        }

        public void WriteFile(ITemplateModel model)
        {
            WriteFile(model.Layout, model);
        }
        public abstract void WriteFile(String template, ITemplateModel model);
    }
}
