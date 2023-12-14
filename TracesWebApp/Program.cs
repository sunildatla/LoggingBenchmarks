using OpenTelemetry.Trace;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry;
using OpenTelemetry.Exporter.Geneva;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services
            .AddControllersWithViews();

        builder.Services
              .AddOpenTelemetry()
              .WithTracing(
            builder =>
            {
                //builder.SetSampler(new AlwaysOnSampler());
                //builder.SetSampler(new AlwaysOffSampler());
                builder.AddAspNetCoreInstrumentation();
                builder.AddHttpClientInstrumentation();

                builder.AddConsoleExporter();
               builder.AddOtlpExporter();
                 builder.SetResourceBuilder(
               ResourceBuilder.CreateDefault()
                   .AddService(serviceName: "my-service-name", serviceVersion: "1.0.0"))
                         .AddSource("*"); ;

                #region GenevaTraceExporter
                //builder.AddGenevaTraceExporter(options =>
                //{
                //    options.ConnectionString = "EtwSession=OpenTelemetrySession";

                //    options.CustomFields = new string[] { "ApplicationName", "ServiceName", "BuildVersion" };
                //}); 
                #endregion
            }
            );

        #region Logging
        //builder.Services.AddLogging(loggingbuilder =>
        //{
        //    loggingbuilder.AddFilter("Azure-Cosmos-Operation-Request-Diagnostics", LogLevel.Trace);
        //    loggingbuilder.AddOpenTelemetry(options =>
        //    {

        //        options.IncludeFormattedMessage = true;
        //        options.IncludeScopes = true;

        //        options.AddConsoleExporter();
        //        options.AddGenevaLogExporter(options =>
        //        {
        //            // options.CustomFields = new string[] { "name" };

        //            options.ConnectionString = "EtwSession=OpenTelemetrySession";

        //            options.TableNameMappings = new Dictionary<string, string>
        //            {
        //                ["*"] = "IntuneOtelEvent"

        //            };

        //        });
        //    });

        //});

        #endregion


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