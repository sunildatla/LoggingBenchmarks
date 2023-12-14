using Microsoft.R9.Extensions.Redaction;
using Microsoft.R9.Extensions.Data.Classification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LoggingBenchmarks.OpenTelemetry
{
    public class LoggerWithRedactor
    {
        public LoggerWithRedactor Instance;
        private readonly ILogger _logger;
        private readonly IRedactorProvider _redactorProvider ;
        private readonly IRedactor _redactor;
        public LoggerWithRedactor(ILogger logger , IRedactorProvider redactorProvider)
        {
            if(null==logger) throw new ArgumentNullException(nameof(logger));
            if(null==redactorProvider) throw new ArgumentNullException(nameof(redactorProvider));

            _logger = logger;
            _redactorProvider= redactorProvider;
            _redactor = _redactorProvider.GetRedactor(DataClass.EUII);

            if(Instance==null)
            Instance= new LoggerWithRedactor(_logger, _redactorProvider); 
        }

        public void LogMessage(string userId, string message) { 
        
          _logger.Log(LogLevel.Information, _redactor.Redact(userId), message);
        }
    }
}
