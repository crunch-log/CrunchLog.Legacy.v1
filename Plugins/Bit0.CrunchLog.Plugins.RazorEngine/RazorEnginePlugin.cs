using Bit0.CrunchLog.Template;
using Bit0.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Bit0.CrunchLog.Plugins.RazorEngine
{
    [Plugin(Name = "Bit0.CrunchLog.Plugins.RazorEngine", Id = "Bit0.CrunchLog.Plugins.RazorEngine", Version = "", Implementing = typeof(ITemplateEngine))]
    public class RazorEnginePlugin : PluginBase
    {
        public RazorEnginePlugin()
        {
        }

        public override IServiceCollection Register(IServiceCollection services)
        {
            services.AddSingleton<ITemplateEngine, RazorTemplateEngine>();
            return services;
        }
    }
}
