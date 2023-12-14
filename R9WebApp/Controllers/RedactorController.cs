using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Metering;
using Microsoft.R9.Extensions.Redaction;
using Microsoft.R9.Extensions.Data.Classification;
using R9WebApp.Processor;

namespace R9WebApp.Controllers
{

    [Route("Redactor")]
    public class RedactorController : Controller
    {
        private readonly ILogger<RedactorController> _logger;
        private readonly IRedactorProvider _redactorProvider;
        private readonly IRedactionProcessor _redacProcessor;
        private readonly IRedactor _redactor;
        private readonly IMeterProvider _meterProvider;
        private readonly ICounter<long> _redactorMeter;
        public RedactorController(ILogger<RedactorController> logger,  IRedactorProvider redactorProvider,IRedactionProcessor redacProcessor, IMeterProvider meterProvider)
        {
            _logger = logger;
            _redactorProvider = redactorProvider;
            _redacProcessor = redacProcessor;
            _redactor = _redactorProvider.GetRedactor(DataClass.EUII);
            _meterProvider = meterProvider;
    
           _redactorMeter= _meterProvider.GetMeter<RedactorController>().CreateCounter("RedactorMeter", "Name", "Occupation", "UserId", "Salary");


        }
        [Route("Index")]
        public IActionResult Index()
        {
            FastLog.UserAvailabilityChanged(_logger, _redactorProvider, "SunilDatla", "Ok");
            _redacProcessor.ProcessMessage();

            _redactorMeter.Add(1, "Sunil", "SoftwareEngineer", "123", "100000");
            return View();
        }
    }
}
