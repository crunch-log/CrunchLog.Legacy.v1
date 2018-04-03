using System;
using Bit0.CrunchLog.TemplateModels;

namespace Bit0.CrunchLog.Theme
{
    public interface IThemeHandler
    {
        void WriteFile(String template, ITemplateModel model);
        void InitOutput();
    }
}