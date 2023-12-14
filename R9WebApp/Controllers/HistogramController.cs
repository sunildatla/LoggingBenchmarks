using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Metering;
using OpenTelemetry.Metrics;
using R9WebApp.Controllers;

namespace R9WebApp
{
    [Route("Histogram")]
    public class HistogramController : Controller
    {
        private readonly IHistogram<long> _latencyMetric;

        private readonly ILogger<HistogramController> _logger;
        public HistogramController(ILogger<HistogramController> logger, IMeterProvider meterprovider, IMeter meter)
        {
            _latencyMetric= StaticMeterProvider.meterProvider.GetMeter("abc").CreateHistogram("Latency", "RequestName", "RequestStatus");
           
            _logger = logger;


        }
        [Route("Index")]
         public IActionResult Index()
        {
            _logger.LogInformation("In HistogramController's Index View");
            _latencyMetric.Record(10, "SupportRequest", "200");
      
            return View();
        }
    }
}
