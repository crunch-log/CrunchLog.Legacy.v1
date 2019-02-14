using Bit0.CrunchLog.Template.Models;

namespace Bit0.CrunchLog.Template
{
    public interface ITemplateEngine
    {
        void Render(ITemplateModel model);
    }
}
