using Bit0.CrunchLog.Template;
using Bit0.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bit0.CrunchLog.Plugins.ScribanEngine
{

    [Plugin(Name = "ScribanEngine", Id = "Bit0.CrunchLog.Plugins.ScribanEngine", Version = "1.0.0", Implementing = typeof(ITemplateEngine))]
    public class ScribanEnginePlugin : PluginBase
    {
        public override IServiceCollection Register(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<ITemplateEngine, ScribanTemplateEngine>());
            return services;
        }
    }
}
