using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var _tracerProvider = Sdk.CreateTracerProviderBuilder()
           .AddHttpClientInstrumentation()
           // Other configuration, like adding an exporter and setting resources
           .AddConsoleExporter()
           .AddOtlpExporter()
           .AddSource("*")
           .SetResourceBuilder(
               ResourceBuilder.CreateDefault()
                   .AddService(serviceName: "my-service-name", serviceVersion: "1.0.0"))

           .Build();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
