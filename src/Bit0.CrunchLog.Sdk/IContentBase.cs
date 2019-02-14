using System;

namespace Bit0.CrunchLog
{
    public interface IContentBase
    {
        Layouts Layout { get; set; }
        String Permalink { get; set; }
        String Title { get; set; }
    }
}