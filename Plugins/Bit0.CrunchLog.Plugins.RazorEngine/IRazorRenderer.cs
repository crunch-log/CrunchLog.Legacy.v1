using Bit0.CrunchLog.Template.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bit0.CrunchLog.Plugins.RazorEngine
{
    public interface IRazorRenderer
    {
        Task RenderViewAsync<TModel>(String viewName, TModel model, TextWriter writer) where TModel : ITemplateModel;
    }
}