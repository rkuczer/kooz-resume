using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace tests
{
    public class ListLogger : ILogger
    {
        public IList<string> Logs;

        public ListLogger()
        {
            this.Logs = new List<string>();
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel,
                              EventId eventId,
                              TState state,
                              Exception? exception,
                              Func<TState, Exception?, string> formatter)
        {
            string message = formatter(state, exception);
            this.Logs.Add(message);
        }
    }
}