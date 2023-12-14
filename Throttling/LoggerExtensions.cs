using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.R9.Extensions.Logging;
using Microsoft.R9.Extensions.Logging.Exporters;
using Microsoft.R9.Extensions.Metering;
using OpenTelemetry;
using OpenTelemetry.Exporter.Geneva;
using OpenTelemetry.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Throttling
{
    internal static class LoggerExtensions
    {
        public static IServiceCollection ConfigureIntuneLogging(this IServiceCollection serviceCollection)
        {

            GenevaExporterOptions genevaExporterOptions = new GenevaExporterOptions() {
                ConnectionString = "EtwSession=OpenTelemetrySession",
                
        };

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            var processors = new List<BaseProcessor<LogRecord>>();
            processors.Add(new ThrottlingProcessor());
            processors.Add(new SecondProcessor());
            processors.Add(new SimpleLogRecordExportProcessor(new GenevaLogExporter(genevaExporterOptions)));
            serviceCollection
                           .AddLogging(builder => builder
                           .ClearProviders()
                           .AddFilter("*", LogLevel.Trace)
                           //.AddProcessor(new LogExceptionProcessor(meterProvider))

                           //.AddOpenTelemetryLogging(options =>
                           //{

                           //    options.UseFormattedMessage = true;
                           //    options.IncludeStackTrace = true;
                           //    options.IncludeScopes = true;
                           //})
                          .AddOpenTelemetry(options => options.AddProcessor(new MyCompositeProcessor(processors))
                           .AddConsoleExporter()));
                         

                           ;

            return serviceCollection;
        }
    }
}
