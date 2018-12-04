using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Bit0.CrunchLog.Template.Factory
{
    public class TemplateFactory : ITemplateFactory
    {
        private readonly ILogger<TemplateFactory> _logger;

        public CrunchSite SiteConfig { get; }

        public Theme Theme { get; }

        public ITemplateEngine Engine { get; }

        public TemplateFactory(
            CrunchSite siteConfig,
            ITemplateEngine templateEngine,
            ILogger<TemplateFactory> logger)
        {
            SiteConfig = siteConfig;
            Theme = SiteConfig.Theme;
            Engine = templateEngine;
            _logger = logger;
        }

        public void PreProcess()
        {
            if (!SiteConfig.Paths.OutputPath.Exists)
            {
                SiteConfig.Paths.OutputPath.Create();
                _logger.LogInformation($"Created folder {SiteConfig.Paths.OutputPath.FullName}");
            }

            if (Theme.OutputType == ThemeOutputType.Json && !Theme.Output.Data.Exists)
            {
                Theme.Output.Data.Create();
                _logger.LogInformation($"Created folder {Theme.Output.Data.FullName}");
            }

            foreach (var dir in Theme.Output.Copy["dirs"])
            {
                var from = SiteConfig.Paths.ThemesPath.CombineDirPath(dir);
                var to = SiteConfig.Paths.OutputPath.CombineDirPath(dir);

                from.Copy(to);
            }

            foreach (var file in Theme.Output.Copy["files"])
            {
                var from = SiteConfig.Paths.ThemesPath.CombineFilePath(file);
                var to = SiteConfig.Paths.OutputPath.CombineFilePath(file);

                from.CopyTo(to.FullName, true);
            }
        }

        public void PostProcess()
        {
            ProcessPreCache();
        }

        private void ProcessPreCache()
        {
            var precache = SiteConfig.Paths.ThemesPath
                .GetFiles(Theme.Output.Process["precache"], System.IO.SearchOption.TopDirectoryOnly)
                .FirstOrDefault();
            var to = SiteConfig.Paths.OutputPath.CombineFilePath("js", precache.Name);

            precache.CopyTo(to.FullName);
        }

        public void Render(ITemplateModel model) => Engine.Render(model);

        public void Render(SiteTemplateModel model) => Engine.Render(model);
    }
}
