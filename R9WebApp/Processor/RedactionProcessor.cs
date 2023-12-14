using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry;
using R9WebApp.Controllers;

namespace R9WebApp.Processor
{
    public class RedactionProcessor : IRedactionProcessor
    {
        private readonly ILogger _logger;
        public RedactionProcessor(ILogger<RedactionProcessor> logger)
        {
            _logger = logger;
        }
        public void ProcessMessage()
        {
            _logger.LogInformation("The Request for redaction is processed");
        }
    }

    public class BasicProcessor : BaseProcessor<LogRecord>
    {
        public BasicProcessor()
        {

        }
        public override void OnEnd(LogRecord data)
        {

        }

    }
}
