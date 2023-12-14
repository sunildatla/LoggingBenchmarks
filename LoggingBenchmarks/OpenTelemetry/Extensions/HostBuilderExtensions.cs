using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Logging;
using Microsoft.R9.Extensions.Logging.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.OpenTelemetry.Extensions
{
    /// <summary>
    /// Extension methods for the HostBuilders like asp.net core , cloud services etc which will be called at the startup by default.
    /// </summary>
    public static  class HostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureIntuneLogging(this IWebHostBuilder webHostbuilder)
        {
            webHostbuilder.ConfigureLogging((context, logBuilder) =>
            {
            var r9loggingEnabled = context.Configuration.GetValue<bool>("R9:Logging:Enabled");
            if (r9loggingEnabled)
            {
                    logBuilder
                    .AddOpenTelemetryLogging(options =>
                        {
                            options.UseFormattedMessage = true;
                            options.IncludeStackTrace = true;
                            options.IncludeScopes = true;
                        })
                    .AddGenevaExporter(context.Configuration.GetSection("R9:Logging:GenevaLogExporter"))
                    .AddGenevaExporter(genevaOptions =>
                        {
                            genevaOptions.ConnectionString = "EtwSession=OpenTelemetrySession";
                            genevaOptions.CustomFields = new string[] { "ApplicationName", "ServiceName", "BuildVersion", "ComponentName", "ThreadId", "FirstName", "RelatedActivityId" };
                            genevaOptions.TableNameMappings = new Dictionary<string, string>
                            {
                                ["LoggingBenchmarks.Loggers.LoggerCategory"] = "IntuneEventnew"

                            };
                        });
                }
            });
            return webHostbuilder;
        }

        public static IHostBuilder ConfigureIntuneLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureLogging((context, logBuilder) =>
            {
                var r9loggingEnabled = context.Configuration.GetValue<bool>("R9:Logging:Enabled");
                if (r9loggingEnabled)
                {
                    logBuilder
                    .AddOpenTelemetryLogging(options =>
                    {
                        options.UseFormattedMessage = true;
                        options.IncludeStackTrace = true;
                        options.IncludeScopes = true;
                    })
                    .AddGenevaExporter(context.Configuration.GetSection("R9:Logging:GenevaLogExporter"))
                    .AddGenevaExporter(genevaOptions =>
                    {
                        genevaOptions.ConnectionString = "EtwSession=OpenTelemetrySession";
                        genevaOptions.CustomFields = new string[] { "ApplicationName", "ServiceName", "BuildVersion", "ComponentName", "ThreadId", "FirstName", "RelatedActivityId" };
                        genevaOptions.TableNameMappings = new Dictionary<string, string>
                        {
                            ["LoggingBenchmarks.Loggers.LoggerCategory"] = "IntuneEventnew"

                        };
                    });
                }
            });

            return hostBuilder;
        }

    }
}
