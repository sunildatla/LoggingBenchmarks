using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.R9.Extensions.Logging;
using Microsoft.R9.Extensions.Logging.Exporters;
using Microsoft.R9.Extensions.Metering;
using Microsoft.R9.Extensions.Metering.Collectors;
using Microsoft.R9.Extensions.Redaction;
using Microsoft.R9.Extensions.Data.Classification;
using R9WebApp.Controllers;
using System.Collections.Generic;
using Microsoft.R9.Extensions.HttpClient.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.R9.Service.Middleware;
using Microsoft.R9.Extensions.Enrichment;
using OpenTelemetry.Exporter.Geneva;
using R9WebApp.Processor;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Net.Http;
using System;

namespace R9WebApp
{
    public class Startup
    {
        private static IReadOnlyDictionary<string, object> prepopulatedFields = new Dictionary<string, object>()
            {
                {"env_cloud_tenant","DAMSU01" },
                { "env_cloud_role","MTFabric"},
                { "env_cloud_roleinstance","MTFabricA_in_0"}
            };
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
           // Activity.CurrentChanged += Activity_CurrentChanged  ;
        services.AddControllersWithViews();


            _ = services.AddSingleton<HomeController>()
                        .AddSingleton<HistogramController>()
                        .AddSingleton<IRedactionProcessor,RedactionProcessor>() ;


            _ = services
                .AddRouting();


                


            _ = services
                .AddGenevaMetering((options) =>
                {
                    //By default this is etw for windows and geneva

                    options.Protocol = TransportProtocol.Etw;
                    options.MonitoringAccount = "mstR9Test";
                    options.MonitoringNamespace = "MeteringSample";

                    //R9Meter-Below Overrides are to create multiple meter providers for different categories with configuration by default.
                    options.MonitoringNamespaceOverrides.Add("R9WebApp.Controllers", "mstR9TelemetryTest");
                    options.MonitoringNamespaceOverrides.Add("abc", "abc");
                })
                .AddLogging((builder) =>
                {
                   
                    builder.AddOpenTelemetryLogging((options) =>
                                {
                                    options.UseFormattedMessage = false;
                                    
                                    
                                })
                    .AddProcessor(new BasicProcessor())
                           .AddConsoleExporter()
                           .AddGenevaExporter((options) =>
                               {
                                   options.ConnectionString = "EtwSession=OpenTelemetry";
                                   options.PrepopulatedFields = prepopulatedFields;
                                   options.CustomFields = new string[] { "ApplicationName", "ServiceName", "BuildVersion", "httpMethod","httpHost","httpStatusCode","httpPath" };
                                   options.TableNameMappings = new Dictionary<string, string>()
                                   {
                                       ["*"] = "IntuneEvent"
                                   };
                               })
                           ;
                })
                .AddDefaultHttpClientLogging()
                .AddRedaction((redaction) =>
                               {
                                   redaction.SetRedactor<YellingRedactor>(DataClass.EUII, DataClass.EUPI);
                               });

            //R9 Extensions to Enrich Logs with Process Information
            //Service Log Enrichers
            //Sending registered system counters by default.
            _ = services.AddEventCounterCollector(options =>
            {
                options.Counters.Add("System.Runtime", new HashSet<string> { "cpu-usage", "working-set" });
                options.Counters.Add("Microsoft.AspNetCore.Hosting", new HashSet<string> { "requests-per-second", "total-requests" });
            })
                        .AddProcessLogEnricher()
                        .AddHttpLogging()
                        .AddServiceLogEnricher(configure => {
                               configure.EnvironmentName = true;
                               configure.ApplicationName = true;
                           });

            //Setting up TraceProvier with exporter.
            _ =  services.AddOpenTelemetryTracing((builder) =>
                {
                  builder
                  .AddSource("*")
                  .AddAspNetCoreInstrumentation()
                 .AddHttpClientInstrumentation()
                  .AddConsoleExporter()
                  .AddGenevaTraceExporter(o =>
                    {
                        o.ConnectionString = "EtwSession=OpenTelemetrySession";
                    })
                    .AddTraceEnricher<MyTraceEnricher>()
                    ;
                });
            
            //R9Meter
            SetStaticMeterProvider(services);
        }

       



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection()
               .UseStaticFiles()
               .UseRouting()
               .UseHttpLoggingMiddleware()
               .UseAuthorization()
               .UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
        }

    
        
        //R9Meter- This is to create a static MeterProvider which can then be used across the classes to create meters. This is only one way of doing it , We can also use DI with IMeterProvider in the classes and create meters too.
        private void SetStaticMeterProvider(IServiceCollection serviceCollection)
        {
            var sp = serviceCollection.BuildServiceProvider();
            StaticMeterProvider.meterProvider = sp.GetRequiredService<IMeterProvider>();

        }

    }


}
