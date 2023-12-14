using LoggingBenchmarks.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter.Geneva;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.OpenTelemetry.Extensions
{
    internal static class TraceExtensions
    {
        public static IServiceCollection AddIntuneTracing(this IServiceCollection serviceCollection, Action<TracerProviderBuilder> configure)
        {
            serviceCollection.AddOpenTelemetryTracing(configure);
            return serviceCollection;
        }
        public static IServiceCollection AddIntuneTracing(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOpenTelemetryTracing(builder =>
             {
                 Sdk.CreateTracerProviderBuilder()
                   .AddSource("*")
                   .AddSource("Samples.SampleServer")
                   .AddSource("Samples.SampleClient")
                   .AddSource("Microsoft.Aspnet.Core")

                   //  .AddProcessor(new FilteringProcessor(new CommonTagProcessor("BasicProcessor"),(act)=>true))
                   .AddHttpClientInstrumentation((options) =>
                   {
                       options.Filter = (httpRequestMessage) =>
                       {
                           return httpRequestMessage.Method.Equals(HttpMethod.Post);
                       };

                       options.Enrich = (activity, eventName, rawObject) =>
                       {
                           if (eventName.Equals("OnStartActivity"))
                           {
                               if (rawObject is HttpRequestMessage request)
                               {
                                   activity.SetTag("requestVersion", request.Version);
                               }
                           }
                           else if (eventName.Equals("OnStopActivity"))
                           {
                               if (rawObject is HttpResponseMessage response)
                               {
                                   activity.SetTag("responseVersion", response.Version);
                               }
                           }
                           else if (eventName.Equals("OnException"))
                           {
                               if (rawObject is Exception exception)
                               {
                                   activity.SetTag("stackTrace", exception.StackTrace);
                               }
                           }
                       };
                   })
                   .AddConsoleExporter()
                   .AddGenevaTraceExporter((options) =>
                   {
                       options.ConnectionString = "EtwSession=OpenTelemetrySession";
                       options.PrepopulatedFields = new Dictionary<string, object>()
                       {
                           [SysVariables.Tenant] = "DAMSU01",
                           [SysVariables.Role] = "MTFabric",
                           [SysVariables.RoleInstance] = "MTFabric_IN_0"
                       };
                       options.CustomFields = new string[] { "ApplicationName", "ServiceName", "BuildVersion" };
                       options.TableNameMappings = new Dictionary<string, string>()
                       {
                          {"Span","IntuneOperation"
                          }
                       };
                   }).

                   Build();
             });
            return serviceCollection;
        }
    }
}
