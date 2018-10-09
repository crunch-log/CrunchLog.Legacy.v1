using Bit0.CrunchLog.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bit0.CrunchLog.Plugin.TemplateEngines.Handlebars
{
    [Plugin(Name = "Handlebars Template Plugin")]
    public class HandlebarsPlugin : ICrunchPlugin
    {
        public void Register(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(ITemplateEngine), typeof(HandelbarsTemplateEngine)));
        }
    }
}
