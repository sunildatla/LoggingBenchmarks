using LoggingBenchmarks.OpenTelemetry.UnifiedApi;
using LoggingBenchmarks.Processors;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Management.Services.Diagnostics;
using Microsoft.Management.Services.Diagnostics.UnifiedApi;
using Microsoft.Management.Services.Diagnostics.UnifiedApi.Contract;
using Microsoft.Management.Services.Diagnostics.UnifiedApi.IfxImplementation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntuneLogInitializer = LoggingBenchmarks.OpenTelemetry.UnifiedApi.IntuneLogInitializer;

namespace LoggingBenchmarks.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public  static class StaticIntuneLogger
    {
        public static Microsoft.Extensions.Logging.ILogger<LoggerCategory> _logger;
        
        static StaticIntuneLogger()
        {
            _logger = IntuneLogInitializer._logger;
         
        }
      
        public static void LogEventWithTraditionalLogger(string componentName, string eventUniqueName, int threadId, params KeyValuePair<string, string>[] columns)
        {
            _logger.LogInformation("Component is :{componentName} and UniqueName is :{eventUniqueName} and kvpars as {KVPair}", componentName, eventUniqueName,columns);

            if(IntuneLogInitializer.isOtelLoggingEnabled)
            LogEventWithR9FastLogger(componentName,eventUniqueName, threadId, columns);
        }

        /// <summary>
        /// Below method can be used to use open telemetry using R9 Fast loggers
        /// by keeping the same IntuneLogger.LogEvent and changing the underlying logging 
        /// by  enabling a flag at initialization which determines which way logging to use.
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="eventUniqueName"></param>
        /// <param name="threadId"></param>
        /// <param name="columns"></param>
        public static void LogEventWithR9FastLogger(string componentName, string eventUniqueName, int threadId, params KeyValuePair<string, string>[] columns)
        {
            LogEvent log = new LogEvent()
            {
                ComponentName = componentName,
                EventUniqueName = eventUniqueName,
                ThreadId = threadId
                
            };

            log.Columns[0]=new MyDictionaryWrapper(new KeyValuePair<string, string>("key1", "Value1"));
            log.Columns[1]=new MyDictionaryWrapper(new KeyValuePair<string, string>("key2", "Value2"));
       //     IntuneFastLogger.LogEvent(_logger, log);

        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerContext"></param>
        /// <param name="componentName"></param>
        /// <param name="eventUniqueName"></param>
        /// <param name="threadId"></param>
        /// <param name="columns"></param>
        public static void LogEventWithR9Logger(ILoggerContext loggerContext, string componentName, string eventUniqueName, int threadId, params KeyValuePair<string, string>[] columns)
        {
            LogEvent log = new LogEvent()
            {
                ComponentName = componentName,
                EventUniqueName = eventUniqueName,
                ThreadId = threadId
            };

            log.Columns[0] = new MyDictionaryWrapper(new KeyValuePair<string, string>("key1", "Value1"));
            log.Columns[1] = new MyDictionaryWrapper(new KeyValuePair<string, string>("key2", "Value2"));

            ILogEvent logEvent = new Microsoft.Management.Services.Diagnostics.UnifiedApi.IfxImplementation.LogEvent(componentName, eventUniqueName);
            logEvent.ActivityId = loggerContext.ActivityId.ToString();
            logEvent.RelatedActivityId = loggerContext.RelatedActivityId.ToString();
            logEvent.SessionId = loggerContext.SessionId.ToString();
            logEvent.cV = loggerContext.GetCurrent_CorrelationVector();
            log.logEvent = logEvent;

            
        //    IntuneFastLogger.LogEvent(_logger, LogLevel.Information, log);

        }


    }

}
