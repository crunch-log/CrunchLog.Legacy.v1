using System;
using System.Collections.Generic;
using System.Text;

namespace Bit0.CrunchLog.Config
{
    public interface IMenuItem
    {
        String Title { get; set; }
        String Url { get; set; }
    }
}
