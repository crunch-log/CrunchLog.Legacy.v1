using System;

namespace Bit0.CrunchLog
{
    public interface IContent
    {
        String Layout { get; set; }
        String Permalink { get; set; }
        String Title { get; set; }
    }
}