using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli
{
    public interface ICliApp
    {
        String BasePath { get; }
        LogLevel VerboseLevel { get; }
    }
}