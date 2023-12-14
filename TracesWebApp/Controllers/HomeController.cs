using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TracesWebApp.Models;

namespace TracesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static readonly ActivitySource activitySource = new("Intune.Traces");
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var httpClient = new HttpClient();

            using (var activity = activitySource.StartActivity("FirstActivity"))
            {

                    
            //        request = HttpContext.Request;
            //    request.Headers.
            //    HttpResponseMessage responseMessage = httpClient.GetAsync("https://api.publicapis.org/entries").GetAwaiter().GetResult();
            //    var result = responseMessage.Content.ReadAsStringAsync().Result;
            //    IEnumerable<string> headernames;
            //     responseMessage.RequestMessage?.Headers.TryGetValues("traceparent", out  headernames);

            //    activity?.AddTag("Response1", result.Substring(0, 10));


            }

            //Activity already created by the aspnet core
            Activity.Current?.SetTag("CustomKey", "CustomValue");
            
                using (var activity = activitySource.StartActivity("InsideIndexofHomeController"))
            {
                
                if (activity != null && activity.IsAllDataRequested == true)
                {
                    activity?.SetTag("Controller", "Home");
                    activity?.SetTag("View", "Index");

                    _logger.LogInformation("This is a test event inside a trace");
                }

            }
            return View();
        }

        public IActionResult Privacy()
        {
            var httpClient = new HttpClient();

            #region Propogation

            using (var activity = activitySource.StartActivity("FirstActivity"))
            {

             
                HttpResponseMessage responseMessage = httpClient.GetAsync("https://api.publicapis.org/entries").GetAwaiter().GetResult();
                var result = responseMessage.Content.ReadAsStringAsync().Result;

                var value= responseMessage.RequestMessage?.Headers.GetValues("avc").ToString();

                activity?.AddTag("Response1", result.Substring(0, 10));


            }
            #endregion

            #region Baggage
            Activity.Current = new Activity("abc");
            using (var activity = activitySource.StartActivity("SecondActivity"))
            {
                Baggage.SetBaggage("AccountId", Guid.NewGuid().ToString());

                HttpResponseMessage responseMessage = httpClient.GetAsync("https://api.publicapis.org/entries").GetAwaiter().GetResult();
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                activity?.AddTag("Response2", result.Substring(0, 10));

            }

            #endregion

            #region Links
            //var activityLinks = new List<ActivityLink>();

            //var linkedContext1 = new ActivityContext(
            //    ActivityTraceId.CreateFromString("0af7651916cd43dd8448eb211c80319c"),
            //    ActivitySpanId.CreateFromString("b7ad6b7169203331"),
            //    ActivityTraceFlags.None);

            //var linkedContext2 = new ActivityContext(
            //    ActivityTraceId.CreateFromString("4bf92f3577b34da6a3ce929d0e0e4736"),
            //    ActivitySpanId.CreateFromString("00f067aa0ba902b7"),
            //    ActivityTraceFlags.Recorded);

            //activityLinks.Add(new ActivityLink(linkedContext1));
            //activityLinks.Add(new ActivityLink(linkedContext2));

            //var linkactivity = activitySource.StartActivity(
            //    "ActivityWithLinks",
            //    ActivityKind.Server,
            //    default(ActivityContext), null,
            //    activityLinks);

            //linkactivity?.SetTag("LinksExample", "IamPartOfTwoTraces");

            //linkactivity?.Stop();
            #endregion


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}