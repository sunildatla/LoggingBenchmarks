using LoggingBenchmarks.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.R9.Extensions.Logging.Exporters;
using Microsoft.R9.Extensions.Metering;
using OpenTelemetry;
using OpenTelemetry.Exporter.Geneva;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using System;
using System.Collections.Generic;

namespace LoggingBenchmarks.OpenTelemetry
{
    internal static class MetricExtensions
    {


        public static IServiceCollection AddIntuneMetrics(this IServiceCollection serviceCollection, Action<MeterProviderBuilder> configure)
        {
            //serviceCollection.AddOpenTelemetryMetrics(configure);
            return serviceCollection;
        }
        public static IServiceCollection AddIntuneMetrics(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddGenevaMetering(options =>
            {
               
                options.Protocol = TransportProtocol.Etw;
                options.MonitoringAccount = "mstR9Test";
                options.MonitoringNamespace = "MeteringSample";
            });

            return serviceCollection; 
                
            // serviceCollection.AddOpenTelemetryMetrics(options =>
            //{
            //    Sdk.CreateMeterProviderBuilder()
            //    .AddMeter("LoggingBenchmarks.MyFirstMeter")
            //    .AddConsoleExporter()
            //    .AddGenevaMetricExporter((options) =>
            //    {
            //        options.PrepopulatedMetricDimensions = new Dictionary<string, object>()
            //        {
            //            [LogSystemEnvVariableKeys.Tenant] = "DAMSU01",
            //            [LogSystemEnvVariableKeys.Role] = "MTFabric",
            //            [LogSystemEnvVariableKeys.RoleInstance] = "MTFabric_IN_0"
            //        };
            //        options.ConnectionString = "Account=MetricAccount;Namespace=MetricNamespace";

            //    })
               
            
        }
    }
}