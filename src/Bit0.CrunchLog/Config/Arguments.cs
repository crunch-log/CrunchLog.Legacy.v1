using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Bit0.CrunchLog.Config
{
    public class Arguments : Dictionary<String, Object>
    {
        public String BasePath
        {
            get => this["basePath"] as String;
            set => this["basePath"] = value;
        }

        public LogLevel VerboseLevel
        {
            get => (LogLevel) this["verboseLevel"];
            set => this["verboseLevel"] = value;
        }

        public Arguments()
        {
            BasePath = ".";
            VerboseLevel = LogLevel.Information;
        }
    }
}
