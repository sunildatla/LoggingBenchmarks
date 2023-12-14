using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Diagnostics;
using System.Net.WebSockets;
using WebAppForLogs.Models;

namespace WebAppForLogs.Controllers
{
    public record Product(
    string id,
    string name
);

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            //CosmosClient client = new CosmosClient("AccountEndpoint=https://testazurestorageautotelemetry.documents.azure.com:443/;AccountKey=WH2rD9TXlucnWP8epIgVJY8bf2VLi2EYkeZaoiLMx5shGiWvMHssP5woFD9wd8jUG8kayTEZTSCQACDb9VQUZQ==;");
            //var container = client.GetContainer("ToDoList", "Items");

            // var res=   container.CreateItemAsync(new Product("100", "datla"), PartitionKey.None).Result;



            //var httpClient = new HttpClient(new SocketsHttpHandler()
            //{
            //    ActivityHeadersPropagator = DistributedContextPropagator.CreateDefaultPropagator()
            //});

            _logger.LogInformation("this is the {value}", "SfC8Q~WWJmyyJIych8XlO.74T-eWpcSAptjcCcrA");

            _logger.LogInformation("Message with attributes {name} - {place} - { age} - { salary} - {country}", "Sunil", "Seattle", 33 ,10000,"USa");


            //HttpResponseMessage responseMessage = httpClient.GetAsync("https://api.publicapis.org/entries").GetAwaiter().GetResult();
            //var cosmosresponse = responseMessage.Content.ReadAsStringAsync().Result;

            //var traceparent = responseMessage.RequestMessage?.Headers?.FirstOrDefault(x => x.Key.Contains("traceparent")).Value != null ? responseMessage.RequestMessage?.Headers?.FirstOrDefault(x => x.Key.Contains("traceparent")).Value.FirstOrDefault() : "";
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("TraceParent from httpclient request header" + traceparent);
            //Console.ForegroundColor = ConsoleColor.White;

            //_logger.LogInformation("After Making HttpCall");




            return RedirectToAction("Privacy");
    ;
    
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("I am in {viewname} View", "Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}