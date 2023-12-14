
using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;

namespace R9Samples
{
    [MemoryDiagnoser]
    public  class BenchmarkTest
    {
        public  ILogger _logger;
        public BenchmarkTest(ILogger logger)
        {
            
            _logger = logger;
        }
        public static void Setup()
        {

        }

        [Benchmark(Baseline = true)]
       public  void BaseTest()
        {
            _logger.LogInformation($"Learn more about {0} at {1}", "OTEL", "REdmond");
            //FastLogger.LearnMoreAtBase(_logger, "OTEL", "Redmond");
        }

        [Benchmark]
        public void R9LoggerTest()
        {
           // FastLogger.LearnMoreAt(_logger, "OTEL", "Redmond");
        }
    }
}
