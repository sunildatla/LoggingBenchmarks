using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.OpenTelemetry
{
    internal class IntuneLoggerScope : IExternalScopeProvider
    {
        public void ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
            throw new NotImplementedException();
        }

        public IDisposable Push(object state)
        {
            throw new NotImplementedException();
        }
    }
}
