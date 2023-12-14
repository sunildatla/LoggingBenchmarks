using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace R9WebApp.Controllers
{
    [Route("Trace")]
    public class TraceController : Controller
    {
        private  readonly ActivitySource activitySource = new ActivitySource("LoggingBenchmarks");
        public TraceController()
        {

        }

        [Route("Index")]
        public IActionResult Index()
        {
            using (var activity = activitySource.StartActivity("Parent"))
            {
                activity?.AddTag("View", "Index");

                using (var childactivity = activitySource.StartActivity("child"))
                {
                    childactivity?.AddTag("View", "childIndex");
                    childactivity?.AddTag("Controller", "childTrace");
                }

                }
               
          return View();
        }
    }
}
