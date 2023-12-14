using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace LoggingBenchmarks.Loggers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class OptimizedLogger
    {
        public readonly ILogger _logger;

        public OptimizedLogger(ILogger logger) => _logger = logger;

        internal static class Events
        {
            public static readonly EventId Started = new EventId(100, "Started");
        }

        public void Log() => OptimizedLog.InformationalMessageNoParams(_logger);

        public void Log(string value1) => OptimizedLog.InformationalMessageOneParam(_logger, value1);

        public void Log(string value1, int value2) => OptimizedLog.InformationalMessageTwoParams(_logger, value1, value2);


        public static void Log(ILogger logger, string value1, string value2, string value3) => LoggerMessage.Define<string,string,string>(LogLevel.Information, new EventId(100), "This is message with three params {value1} {value2} {value3}");


        //[LoggerMessage(EventId = 200,
        //       Level = LogLevel.Critical,
        //       Message = "Got new employee :{emp}")]
        //public static partial void LogComplexObject(ILogger logger, Employee emp);

        //public void LogComplex(Employee emp)
        //{
        //    _logger.Log(LogLevel.Information,default,emp,null,Employee.Formatter);
        //}

        [LoggerMessage(EventId = 100,
               Level = LogLevel.Critical,
               Message = "Could not open Socket to `{param1}` `{param2}`")]
        public partial  void CouldNotOpenSocketOneParam(string param1, string param2);
        
        public void LogDebug(string value1, int value2) => OptimizedLog.DebugMessage(_logger, value1, value2);

        private static class OptimizedLog
        {
            private static readonly Action<ILogger, Exception> _informationLoggerMessageNoParams = LoggerMessage.Define(
                LogLevel.Information,
                Events.Started,
                "This is a message with no params!");

            private static readonly Action<ILogger, string, Exception> _informationLoggerMessageOneParam = LoggerMessage.Define<string>(
                LogLevel.Information,
                Events.Started,
                "This is a message with one param! {Param1}");

            private static readonly Action<ILogger, string, int, Exception> _informationLoggerMessage = LoggerMessage.Define<string, int>(
                LogLevel.Information,
                Events.Started,
                "This is a message with two params! {Param1}, {Param2}");

            private static readonly Action<ILogger, string, int, Exception> _debugLoggerMessage = LoggerMessage.Define<string, int>(
                LogLevel.Debug,
                Events.Started,
                "This is a debug message with two params! {Param1}, {Param2}");

            public static void DebugMessage(ILogger logger, string value1, int value2) => _debugLoggerMessage(logger, value1, value2, null);

            private static readonly Action<ILogger, string, string, string, string, Exception> _debugLoggerMessageFourParam = LoggerMessage.Define<string, string, string, string>(LogLevel.Debug, Events.Started, "This is debug message with four params {Param1},{Param2},{Param3},{Param4}");

            public static void InformationalMessageNoParams(ILogger logger) => _informationLoggerMessageNoParams(logger, null);

            public static void InformationalMessageOneParam(ILogger logger, string value1) => _informationLoggerMessageOneParam(logger, value1, null);

            public static void InformationalMessageTwoParams(ILogger logger, string value1, int value2) => _informationLoggerMessage(logger, value1, value2, null);


            public static void DebugMessageWithFourParam(ILogger logger, string value1, string value2, string value3, string value4) => _debugLoggerMessageFourParam(logger, value1, value2, value3, value4, null);

            public static void DebugMessageWithLevelCheck(ILogger logger, string value1, int value2)
            {
                if (logger.IsEnabled(LogLevel.Debug))
                    _debugLoggerMessage(logger, value1, value2, null);
            }
        }
    }
}