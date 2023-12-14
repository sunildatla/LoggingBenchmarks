using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using System.Diagnostics;
using WebAppForLogs;
using OpenTelemetry.Resources;
using System.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using OpenTelemetry.Exporter.CredScan;
using OpenTelemetry.Exporter.Geneva;

internal class Program
{

    private static void Main(string[] args)
    {
        if(string.Equals("PayLoadId", "payLoadId", StringComparison.OrdinalIgnoreCase))
        {

        }
       
        var builder = WebApplication.CreateBuilder(args);

        //AppContext.SetSwitch("System.Net.Http.EnableActivityPropagation", false);
        AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);
        //  Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        DistributedContextPropagator.Current = new SkipHttpClientActivityPropagator();
        //ActivitySource.AddActivityListener(new ActivityListener()
        //{
        //    ActivityStarted = a =>
        //    {
        //        Console.WriteLine($"ParentId : {a.ParentId} TraceId:{a.TraceId} SpanId: {a.SpanId}");
        //    },
        //    ShouldListenTo = ((a) => { return true; })
        //    ,Sample= (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData

        //});


        //builder.Services.AddOpenTelemetry()
        //     .WithTracing(
        //    builder =>
        //    {
        //        builder.AddAspNetCoreInstrumentation();
        //        builder.AddHttpClientInstrumentation();
        //        //builder.AddHttpClientInstrumentation(options =>
        //        //{
        //        //    options.EnrichWithHttpRequestMessage = (activity, requestmessage) =>
        //        //    {
        //        //        activity.SetTag("requestVersion", requestmessage.Version);
        //        //        activity.SetTag("RequestHeaders", requestmessage.Headers);
        //        //    };
        //        //    options.EnrichWithHttpResponseMessage = (activity, requestmessage) =>
        //        //    {
        //        //        activity.SetTag("ResponseVersion", requestmessage.Version);
        //        //        activity.SetTag("ResponseStatusCode", requestmessage.StatusCode);
        //        //        activity.SetTag("ResponseContent", requestmessage.Content.ReadAsStringAsync().Result);
        //        //    };
        //        //});
        //        builder.AddSource("*");
        //        builder.AddConsoleExporter();
        //        builder.AddOtlpExporter();
        //    }
        //    );

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var loggerFactory = LoggerFactory.Create(builder => builder
                .AddConsole()
                .AddOpenTelemetry(loggerOptions =>
                {
                    loggerOptions.AddGenevaCredScanLogExporter((credscanOptions, genevaOptions) =>
                    {
                        credscanOptions.Mode = Mode.Redact;
                        credscanOptions.SamplingRatio = 0.5;//This will come from Config value.
                        credscanOptions.ScannerProfile= new IScannerProfileOptions[] {
                                                            new UserDefinedRegexOptions(new (string, string)[] { ("CommonCreds", "regex") }),
                                                            new KbProviderOptions(ScannerProviders.CoreLogProvider)
};
                        // Add custom logic to skip scanning certain data.

                        // Set GenevaLogExporter connection string.
                        genevaOptions.ConnectionString = "EtwSession=OpenTelemetrySession";
                        genevaOptions.TableNameMappings = new Dictionary<string, string>
                        {
                            ["*"] = "IntuneOtelEvent"

                        };
                    });
                }));

        

        var logger = loggerFactory.CreateLogger<Program>();
        
   
        //logger.LogInformation("2this is the {value}", "HostNa​me=a​ccou​nt.redis.ca​che.windows.net;P​a​ssword=a​bcdefghijklmnop​qrstu​vwxyz​0123456789/+ABCDE=  \r\n");
        //logger.LogInformation("3this is the {value}", "Endp​oint=ta​ble.core.windows.net;Accou​ntNa​me=a​ccou​nt;Accou​ntKey=a​bcdefghijklmnop​qrstu​vwxyz​0123456789/+ABCDEa​bcd…\r\nAccou​ntNa​me=a​ccou​ntAccou​ntKey=a​bcdefghijklmnop​qrstu​vwxyz​0123456789/+ABCDEa​bcdefghijklmnop​qrstu​vwxyz​0123456789/…\r\nP​rima​ryKey=a​bcdefghijklmnop​qrstu​vwxyz​0123456789/+ABCDEa​bcdefghijklmnop​qrstu​vwxyz​0123456789/+ABCDE==");
        //logger.LogInformation("4this is the {value}", "Endp​oint=sb://a​ccou​nt.servicebu​s.windows.net;Sha​redAccessKey=a​bcdefghijklmnop​qrstu​vwxyz​0123456789/+ABCDE=\r\nServiceBu​sNa​mesp​a​ce=...Sha​redAccessP​olicy=...Key=a​bcdefghijklmnop​qrstu​vwxyz​0123456789/+ABCDE=");

        var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
                
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        
} 

}