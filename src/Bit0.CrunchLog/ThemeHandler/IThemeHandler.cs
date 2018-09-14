using System;
using Bit0.CrunchLog.Template.Models;

namespace Bit0.CrunchLog.ThemeHandler
{
    public interface IThemeHandler
    {
        void WriteFile(String template, ITemplateModel model);
        void WriteFile(ITemplateModel model);
        void InitOutput();
    }
}