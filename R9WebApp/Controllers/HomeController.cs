using Azure.Core.Diagnostics;
using Azure.Data.Tables;
using InstrumentationSdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Metering;
using Microsoft.R9.Extensions.Redaction;
using R9WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace R9WebApp.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ActivitySource source = new ActivitySource("LoggingBenchmarks");
        private readonly MemoryUsage memoryUsage;
        private readonly Requests requests;
        private readonly TotalCount totalCount;
        private readonly ILogger<HomeController> _logger;


        private readonly TableClientOptions clientOptions = new TableClientOptions()
        {
            Diagnostics =
                {
                    IsLoggingContentEnabled = true,
                    LoggedHeaderNames=  { "x-ms-request-id" },
                    LoggedQueryParameters={"api-version"},
                    IsDistributedTracingEnabled = true
                }
        };
      

        public HomeController(ILogger<HomeController> logger, IMeterProvider meterprovider)
        {
            requests = FastMetrics.CreateRequests(meterprovider.GetMeter<HomeController>());
            memoryUsage = FastMetrics.CreateMemoryUsage(meterprovider.GetMeter<HomeController>());
            totalCount = FastMetrics.CreateTotalCount(meterprovider.GetMeter<HomeController>());
            _logger = logger;
            
        }

        /// <summary>
        /// Logger 
        /// </summary>
        /// <returns></returns>
        [Route("Index")]
        public IActionResult Index()
        {
            _logger.LogInformation("In Controller {0} at view {1} {2}","Home","Index",Activity.Current.SpanId);

            _logger.LogInformation("In Controller {controllerName} at view {viewName}", "Home", "Index");


            using (var activity = source.StartActivity("Home/Index", ActivityKind.Server, Activity.Current.Context))
            { 
           
                activity?.SetTag("Col1","ColValue1");

                using (var activity2 = source.StartActivity("Home/Index2", ActivityKind.Server, Activity.Current.Context))
                {
                    _logger.LogInformation("In HomeController's Index 2 View");
                    activity?.SetTag("Col1", "ColValue1");
                }

          

                 return RedirectToAction("StructuredLogging", "Home");
            }
        }

        /// <summary>
        /// Structured Logging
        /// </summary>
        /// <returns></returns>
        [Route("StructuredLogging")]
        public IActionResult StructuredLogging()
        {
            string controllerName = "Home";
            string viewName = "StructuredLogging";

            using (var activity = source.StartActivity("Home/StructuredLogging", ActivityKind.Server, Activity.Current.Context))
            { 
         
                activity?.SetTag("Col2", "Col2Value");

                _logger.LogInformation("In {controllerName} Controllers {viewName} view", controllerName, viewName);
                 return View();
            }

        }

       

        [Route("ComplexObjectLogging")]
        public IActionResult ComplexObjectLogging()
        {
            ProductEntity pEntity = new ProductEntity() { productId = 1, productName = "Apple", PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString(), Timestamp = DateTime.Now };

            FastLog.ProductSent(_logger, pEntity);

            #region commented
            //using AzureEventSourceListener traceListener = AzureEventSourceListener.CreateTraceLogger();

            //using AzureEventSourceListener consoleListener = AzureEventSourceListener.CreateConsoleLogger();
            //using ActivityListener listener = new ActivityListener()
            //{
            //    ShouldListenTo = a => a.Name.StartsWith("Azure"),
            //    Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
            //    SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
            //    ActivityStarted = activity => Console.WriteLine("Start: " + activity.DisplayName),
            //    ActivityStopped = activity => Console.WriteLine("Stop: " + activity.DisplayName)
            //};
            //ActivitySource.AddActivityListener(listener);

            //TableClient client;

            //client.AddEntityAsync(pEntity).ConfigureAwait(false).GetAwaiter().GetResult();
            #endregion

            return View();
        }


        /// <summary>
        /// Gauge Meter
        /// </summary>
        /// <returns></returns>
        [Route("MetricGauge")]
        public IActionResult MetricGauge()
        {
            memoryUsage.Record(Process.GetCurrentProcess().PrivateMemorySize64, "Sandbox", "Redmond", Process.GetCurrentProcess().MachineName, Process.GetCurrentProcess().ProcessName);

            return View();
        }
        /// <summary>
        /// Counter Meter
        /// </summary>
        /// <returns></returns>
        [Route("MetricCounter")]
        public IActionResult MetricCounter()
        {
            requests.Add(1, "Sandbox", "Redmond", "Support", "200");
            return View();
        }
        /// <summary>
        /// Complex Type Meter DImensions
        /// </summary>
        /// <returns></returns>
        [Route("ComplexMetricCounter")]
        public IActionResult ComplexMetricCounter()
        {
            totalCount.Add(1, new MetricDimensions() { dim1 = "CustomDim1", dim2 = "CustomDim2", dim3 = "CustomDim3" });
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
