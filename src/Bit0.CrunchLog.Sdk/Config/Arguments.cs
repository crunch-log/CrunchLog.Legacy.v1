using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Config
{
    public class Arguments
    {
        public String BasePath { get; set; }

        public LogLevel LogLevel { get; set; }

        public Boolean LoadConfig { get; set; }
    }
}
