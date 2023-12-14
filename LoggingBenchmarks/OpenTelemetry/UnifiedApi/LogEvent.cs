using Microsoft.Management.Services.Diagnostics.UnifiedApi.Contract;
using Microsoft.Management.Services.Diagnostics.UnifiedApi.IfxImplementation;
using System;

namespace LoggingBenchmarks.Loggers
{
    /// <summary>
    /// IntuneLogger LogEvent Complex Object which will be converted to before using this in the R9FastLogger
    /// </summary>
    public class LogEvent
    {
        public string ComponentName { get; set; }

        public string EventUniqueName { get; set; }

        public int ThreadId { get; set; }
        
        public MyDictionaryWrapper[] Columns = new MyDictionaryWrapper[2] ;

        public ILogEvent logEvent { get; set; }
    }
}
