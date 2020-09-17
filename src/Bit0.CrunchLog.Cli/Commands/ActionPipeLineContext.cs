using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bit0.CrunchLog.Cli.Commands
{
    public class ActionPipeLineContext<T>
    {
        public CrunchLog CrunchLog { get; }

        public IServiceProvider ServiceProvider { get; }

        public ILogger<T> Logger { get; }

        public ActionPipeLineContext(IServiceProvider provider, ILogger<T> logger, CrunchLog crunchLog)
        {
            ServiceProvider = provider;
            Logger = logger;
            CrunchLog = crunchLog;
        }
    }
}
