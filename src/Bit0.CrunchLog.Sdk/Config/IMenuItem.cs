using System;

namespace Bit0.CrunchLog.Config
{
    public interface IMenuItem
    {
        String Title { get; set; }
        String Url { get; set; }
    }
}
