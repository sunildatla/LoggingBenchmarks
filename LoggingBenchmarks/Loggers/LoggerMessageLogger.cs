using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingBenchmarks.Loggers
{
    public partial class LoggerMessageLogger
    {
        private readonly ILogger _logger;
        public LoggerMessageLogger(ILogger logger)
        {
            _logger = logger;
        }

        [LoggerMessage(EventId = 100,
               Level = LogLevel.Critical,
               Message = "Could not open Socket to `{hostName}`")]
        public partial void CouldNotOpenSocketOneParam(string hostName);


        [LoggerMessage(EventId = 200,
                   Level = LogLevel.Critical,
                   Message = "Could not open Socket to `{hostName}` at Port `{portName}`")]
        public partial void CouldNotOpenSocketTwoParam(string hostName, string portName);
    }
}
