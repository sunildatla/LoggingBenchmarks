using LoggingBenchmarks.Configuration;
using LoggingBenchmarks.Constants;
using LoggingBenchmarks.OpenTelemetry.Processors.Log;
using LoggingBenchmarks.Processors;
//using LoggingBenchmarks.Providers.LoggingProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.R9.Extensions.Data.Classification;
using Microsoft.R9.Extensions.Enrichment;
using Microsoft.R9.Extensions.HttpClient.Logging;
using Microsoft.R9.Extensions.Logging;
using Microsoft.R9.Extensions.Logging.Exporters;
using Microsoft.R9.Extensions.Metering;
using Microsoft.R9.Extensions.Redaction;
using Microsoft.R9.Extensions.Tracing.Http;
using Microsoft.R9.Extensions.Tracing.HttpClient;
using OpenTelemetry;
using OpenTelemetry.Exporter.Geneva;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;

namespace LoggingBenchmarks.OpenTelemetry
{
    public static class LoggerExtensions
    {
        public static IServiceCollection ConfigureIntuneLogging(this IServiceCollection serviceCollection , Action<ILoggingBuilder> configure)
        {
            serviceCollection.AddLogging(configure);
            return serviceCollection;
        }
        public static IServiceCollection ConfigureIntuneLogging(this IServiceCollection serviceCollection)
        {

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var meterProvider = serviceProvider.GetService<IMeterProvider>();
            var logConfigOptionsMonitor = serviceProvider.GetService<IOptionsMonitor<LoggingConfig>>();

            serviceCollection
                           .AddLogging(builder => builder
                           .ClearProviders()
                           .AddFilter("*", LogLevel.Trace)
                          //  .AddProvider(new IntuneOtelLoggerProvider(logConfigOptionsMonitor))
                           .AddProcessor(new LogExceptionProcessor(meterProvider))
                         
                           .AddOpenTelemetryLogging(options =>
                           {
                               
                               options.UseFormattedMessage = true;
                               options.IncludeStackTrace = true;
                               options.IncludeScopes = true;
                           })
                          
                           .AddGenevaExporter(genevaOptions =>
                           {
                               genevaOptions.ConnectionString = "EtwSession=OpenTelemetrySession";
                               //Use this if Values of the fields are fixed like env variables etc
                               genevaOptions.PrepopulatedFields = new Dictionary<string, object>()
                               {
                                   [SysVariables.Tenant] = "DAMSU01",
                                   [SysVariables.Role] = "MTFabric",
                                   [SysVariables.RoleInstance] = "MTFabric_IN_0",
                                   [SysVariables.ServiceName] =
                                   "LoggingBenchmarks",
                                   [SysVariables.Buildversion] ="1.0",
                                   [SysVariables.ApplicationName]="LoggingBenchmarksApplication"
                               };
                               //PartC variables - to avoid schema explosion
                             genevaOptions.CustomFields = new string[] {  ContextVariables.ComponentName,ContextVariables.AccountId,ContextVariables.SessionId
                             ,ContextVariables.ActivityId,ContextVariables.RelatedActivityId,ContextVariables.UserId,ContextVariables.DeviceId,ContextVariables.CV,ContextVariables.CorrelationVector};
                               genevaOptions.TableNameMappings = new Dictionary<string, string>
                               {
                                   ["LoggingBenchmarks.Loggers.LoggerCategory"] = "IntuneEventnew"
                                   //["*"]="IntuneEvent"
                               };
                           })
                           .AddConsoleExporter())
                           .AddCorrelationVectorLogEnricher()
                           .AddServiceLogEnricher(configure => {
                               configure.EnvironmentName = true;
                               configure.ApplicationName = true;
                           })
                           .AddProcessLogEnricher(configure =>
                           {
                               configure.ProcessId = true;
                               configure.ThreadId = true;
                           })

                           ;

            return serviceCollection;
        }


        public static IServiceCollection ConfigureIntuneTracing(this IServiceCollection serviceCollection)
        {
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            serviceCollection.AddOpenTelemetryTracing(builder =>
            {
                builder.AddServiceTraceEnricher()
                
                        .AddHttpTracing(options =>
                        {
                            // options.IncludePath = false; <- this is default value

                            // Add the tags with appropriate data classification you need.
                            // options.RouteParameterDataClasses.Add("userId", DataClass.EUII);
                            // options.RouteParameterDataClasses.Add("mri", DataClass.EUPI);

                            // Exclude all unneeded paths like /health
                            // options.ExcludePathStartsWith.Add("/api/probe/live");
                        })
                        .AddHttpClientTracing(options =>
                        {
                            // Add the tags with appropriate data classification you need.
                            // options.RouteParameterDataClasses.Add("userId", DataClass.EUII);
                            // options.RouteParameterDataClasses.Add("mri", DataClass.EUPI);
                        });
            });
            return serviceCollection;
        }

        //HostBuilder also can be used as another extension to get the context and configuration of the service when they startup the application.
       public static IServiceCollection ConfigureIntuneRedaction(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRedaction(configure =>
            {
                configure.SetXXHashRedactor(
                    configure => { configure.HashSeed = 100; },
                    DataClass.EUII
                    );
                configure.SetO365Redactor(configure =>
                {
                    configure.KeyId = 100;
                    configure.Key = "QkZXSVdETVFXS1FXSUZIV1FKTURXUU9XSUZRTldPREtNUU9JV0ZCSVFLV0Q=";
                }, DataClass.EUPI);
            });
           
            return serviceCollection;
        }
    }
}
