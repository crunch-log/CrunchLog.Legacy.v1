using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog.Config
{
    public class Arguments : Dictionary<String, Object>
    {
        public const String UrlDefault = "http://localhost:3576/";

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

        public String Url
        {
            get => this["url"] as String;
            set => this["url"] = value;
        }

        public Arguments()
        {
            BasePath = ".";
            VerboseLevel = LogLevel.Information;
            Url = UrlDefault;
        }
    }
}
