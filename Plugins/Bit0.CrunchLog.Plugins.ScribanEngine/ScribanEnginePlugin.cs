using Bit0.CrunchLog.Template;
using Bit0.Plugins;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bit0.CrunchLog.Plugins.ScribanEngine
{

    [Plugin(Name = "Bit0.CrunchLog.Plugins.RazorEngine", Id = "Bit0.CrunchLog.Plugins.RazorEngine", Version = "", Implementing = typeof(ITemplateEngine))]
    public class ScribanEnginePlugin : PluginBase
    {
        public override IServiceCollection Register(IServiceCollection services)
        {
            services.AddSingleton<ITemplateEngine, ScribanTemplateEngine>();
            return services;
        }
    }
}
