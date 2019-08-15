using Bit0.CrunchLog.Template;
using Bit0.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Bit0.CrunchLog.Plugins.ScribanEngine
{

    [Plugin(Name = "ScribanEngine", Id = "Bit0.CrunchLog.Plugins.ScribanEngine", Version = "1.0.0", Implementing = typeof(ITemplateEngine))]
    public class ScribanEnginePlugin : PluginBase
    {
        public override IServiceCollection Register(IServiceCollection services)
        {
            services.AddSingleton<ITemplateEngine, ScribanTemplateEngine>();
            return services;
        }
    }
}
