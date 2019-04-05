using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;
using Microsoft.Extensions.Logging;
using RazorLight;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bit0.CrunchLog.Plugins.RazorEngine
{
    public class RazorRenderer : IRazorRenderer
    {
        private readonly RazorLightEngine _razorEngine;
        private readonly ILogger<IRazorRenderer> _logger;

        public RazorRenderer(
            CrunchSite siteConfig,
            ILogger<IRazorRenderer> logger)
        {
            _logger = logger;
            var themeDir = siteConfig.Paths.ThemesPath;

            _logger.LogInformation("Initialize RazorEngine");

            _razorEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(themeDir.CombineDirPath("Views").FullName)
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task RenderViewAsync<TModel>(String viewName, TModel model, TextWriter writer) where TModel : ITemplateModel
        {
            _logger.LogInformation($"Render view: {viewName}");

            var content = await _razorEngine.CompileRenderAsync(viewName, model);
            await writer.WriteAsync(content);
        }
    }
}
