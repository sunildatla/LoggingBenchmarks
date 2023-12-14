using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var httpClient = new HttpClient();


            HttpResponseMessage responseMessage = httpClient.GetAsync("https://api.publicapis.org/entries").GetAwaiter().GetResult();
            var cosmosresponse = responseMessage.Content.ReadAsStringAsync().Result;

            var traceparent = responseMessage.RequestMessage?.Headers?.FirstOrDefault(x => x.Key.Contains("traceparent")).Value != null ? responseMessage.RequestMessage?.Headers?.FirstOrDefault(x => x.Key.Contains("traceparent")).Value.FirstOrDefault() : "";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("TraceParent from httpclient request header" + traceparent);
            Console.ForegroundColor = ConsoleColor.White;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}