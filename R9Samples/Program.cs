using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Data.Classification;
using Microsoft.R9.Extensions.Enrichment;
using Microsoft.R9.Extensions.Logging;
using Microsoft.R9.Extensions.Logging.Exporters;
using Microsoft.R9.Extensions.Tracing.Http;
using OpenTelemetry.Exporter.Geneva;
using R9Samples.Enrichments;
using System;

namespace R9Samples
{
    internal class Program
    {
        /// <summary>
        /// USAGE: R9Samples.exe -f "*Benchmarktest*" 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(
                                         (builder) => builder.AddOpenTelemetryLogging()
                                                             .AddConsoleExporter()
                         )
                             .AddProcessLogEnricher();



            ILogger logger=   serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>().CreateLogger("LoggingBenchmarks");

            BenchmarkTest benchmarkTest = new BenchmarkTest(logger);
            benchmarkTest.BaseTest();
            benchmarkTest.R9LoggerTest();

            //FastLogger.LearnMoreAt(logger, "OTEL", "Redmond");
            //FastLogger.LearnMoreAtBase(logger, "OTEL", "Redmond");

            Console.Read();

        }

       
    }
}
