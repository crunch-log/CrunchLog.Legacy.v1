using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Bit0.CrunchLog.ThemeHandler
{
    public abstract class ThemeHandlerBase : IThemeHandler
    {
        protected ThemeHandlerBase(CrunchSite siteConfig, JsonSerializer jsonSerializer)
        {
            SiteConfig = siteConfig;

            var themeMeta = SiteConfig.Theme.CombineFilePath(".json", "theme");
            var theme = new Theme(themeMeta);
            jsonSerializer.Populate(themeMeta.OpenText(), theme);

            Theme = theme;
        }

        protected CrunchSite SiteConfig { get; }
        protected Theme Theme { get; } 

        public void InitOutput()
        {
            if (!SiteConfig.Paths.OutputPath.Exists)
            {
                SiteConfig.Paths.OutputPath.Create();
            }

            foreach (var dir in Theme.Assets.Directories.Select(d => Theme.Directory.CombineDirPath(d)))
            {
                dir.Copy(SiteConfig.Paths.OutputPath.CombineDirPath(dir.Name));
            }

            foreach (var file in Theme.Assets.Files.Select(f => Theme.Directory.CombineFilePath(f)))
            {
                file.CopyTo(SiteConfig.Paths.OutputPath.CombineDirPath(file.Name).FullName, true);
            }
        }

        public void WriteFile(ITemplateModel model)
        {
            WriteFile(model.Layout, model);
        }
        public abstract void WriteFile(String template, ITemplateModel model);
    }
}
