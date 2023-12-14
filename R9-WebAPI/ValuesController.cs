using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace R9_WebAPI
{
    [Route("api/Values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MemoryUsage memoryUsage;
        public ValuesController()
        {

        }

    }
}
