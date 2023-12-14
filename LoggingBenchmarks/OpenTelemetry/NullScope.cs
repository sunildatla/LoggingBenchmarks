using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.OpenTelemetry
{
    /// <summary>
    /// Empty Scope without any logic when the scopes are turned off -default
    /// </summary>
    internal class NullScope : IDisposable
    {
       public static  NullScope Instance { get; }= new NullScope();
       internal NullScope() { }
        public void Dispose()
        {
        }
    }
}
