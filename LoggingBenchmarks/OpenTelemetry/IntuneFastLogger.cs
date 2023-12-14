using Microsoft.Extensions.Logging;
using Microsoft.Management.Services.Diagnostics;
using Microsoft.R9.Extensions.Data.Classification;
using Microsoft.R9.Extensions.Logging;
using Microsoft.R9.Extensions.Redaction;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.Loggers
{
    /// <summary>
    /// 1.Fixed Schema - Strongly typed methods
    /// 2.Locked Column Structure
    /// 3.Fast & Structured logging & recommended way
    /// 4.Non Generic
    /// 5.LogProperties can take multiple Complex objects as parameters
    /// **6.Lock the category for the ILogger in fast logging so that it always goes to same table in kusto -any other logger initialized with different category fails at build time.
    /// </summary>
    public static partial class IntuneFastLogger
    {
      
        //[LogMethod(1, LogLevel.Information, "No params here...")]
        //public static partial void LogEvent(
        //    ILogger<LoggerCategory> logger, 
        //    [LogProperties(OmitParameterName = true,SkipNullProperties =true)] LogEvent unifiedApiLog);

        //[LogMethod(2, "UnifiedApiLog is logged here'")]
        //public static partial void LogEvent(
        //    ILogger<LoggerCategory> logger, 
        //    LogLevel level, 
        //    [LogProperties(OmitParameterName = true, SkipNullProperties = true)] LogEvent unifiedApiLog);


        //[LogMethod(3)]
        //public static partial void LogEvent(
        //    ILogger<LoggerCategory> logger, 
        //    LogLevel level,
        //    [LogProperties(OmitParameterName =true,SkipNullProperties =true)] LoggerContext loggerContext,[LogProperties(OmitParameterName = true, SkipNullProperties = true)] LogEvent unifiedApiLog,
        //    string logMesage);

        //[LogMethod(4)]
        //public static partial void LogEventWithRedactor
        //    (ILogger<LoggerCategory> logger,
        //    IRedactorProvider redactorProvider,
        //    LogLevel level,
        //    [LogProperties(OmitParameterName = true, SkipNullProperties = true)] LoggerContext loggerContext,[LogProperties(OmitParameterName = true, SkipNullProperties = true)] LogEvent unifiedApiLog,
        //    [EUPI] string logMesage);

        //[LogMethod(5)]
        //public static partial void LogEventWithRedactorEUII
        //    (ILogger<LoggerCategory> logger,
        //    IRedactorProvider redactorProvider,
        //    LogLevel level,
        //    [LogProperties(OmitParameterName = true, SkipNullProperties = true)] LoggerContext loggerContext, [LogProperties(OmitParameterName = true, SkipNullProperties = true)] LogEvent unifiedApiLog,
        //    [EUII] string logMesage);



    }


}
