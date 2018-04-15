using System;
using Bit0.CrunchLog.TemplateModels;

namespace Bit0.CrunchLog.ThemeHandler
{
    public interface IThemeHandler
    {
        void WriteHtml(String template, ITemplateModel model);
        void WriteCss(String template, ITemplateModel model);
        void WriteFile(String template, ITemplateModel model);
        void InitOutput();
    }
}