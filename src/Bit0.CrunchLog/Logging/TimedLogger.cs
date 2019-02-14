using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Logging
{
    public class TimedLogger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public TimedLogger(ILogger logger)
        {
            _logger = logger;
        }

        public TimedLogger(ILoggerFactory loggerFactory) : this(new Logger<T>(loggerFactory)) { }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, String> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, (s, ex) => $"[{DateTime.UtcNow:HH:mm:ss.fff}]: {formatter(state, exception)}");
        }

        public Boolean IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }
    }
}
