using BenchmarkDotNet.Loggers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace LoggingBenchmarks.Loggers
{
    public class StandardLogger
    {
        private readonly ILogger _logger;

        public StandardLogger(ILogger logger) => _logger = logger;

        public void LogOnceWithNoParam() =>
            _logger.LogInformation("This is a message with no params!");

        public void LogOnceWithOneParam(string value1) =>
            _logger.LogInformation("This is a message with one param! {Param1}", value1);



        public void LogWithScope(string value)
        {
            using (_logger.BeginScope(new Dictionary<string, object> { { "PersonId", 5 } }))
            {
                _logger.LogInformation("Hello");
                _logger.LogInformation("World");
                _logger.LogInformation(value);
                
            }
            
        }

        public void LogOnceWithTwoParams(string value1, int value2) =>
            _logger.LogInformation("This is a message with two params! {Param1}, {Param2}", value1, value2);

        public void LogDebugOnceWithTwoParams(string value1, int value2) =>
            _logger.LogDebug("This is a debug message with two params! {Param1}, {Param2}", value1, value2);

        public void Log(string message, LogLevel logLevel) =>
            _logger.Log(logLevel: logLevel,
            eventId: default,
            state: new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("ApplicationName", "LoggingBenchmarks"),
                new KeyValuePair<string, object>("ServiceName", "OpenTelemetry"),
                new KeyValuePair<string,object>("BuildVersion",1)
            },
            exception: null,
            formatter: (state, ex) => "message"
            );
    }
}