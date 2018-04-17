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

            var themeMeta = Config.Site.Theme.CombineFilePath(".json", "theme");
            var theme = new Theme(themeMeta);
            jsonSerializer.Populate(themeMeta.OpenText(), theme);

            Theme = theme;
        }

        protected CrunchConfig Config { get; }
        protected Theme Theme { get; } 

        public void InitOutput()
        {
            if (!Config.Paths.OutputPath.Exists)
            {
                Config.Paths.OutputPath.Create();
            }

            foreach (var dir in Theme.Assets.Directories.Select(d => Theme.Directory.CombineDirPath(d)))
            {
                dir.Copy(Config.Paths.OutputPath.CombineDirPath(dir.Name));
            }

            foreach (var file in Theme.Assets.Files.Select(f => Theme.Directory.CombineFilePath(f)))
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
