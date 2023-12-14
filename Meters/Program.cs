// See https://aka.ms/new-console-template for more information
using OpenTelemetry;
using OpenTelemetry.Exporter.Geneva;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;

internal class Program
{

    private static readonly Meter myFirstMeter = new Meter("MyFirstMeter");

    private static readonly Counter<long> iCounter = myFirstMeter.CreateCounter<long>("MyFirstCounter");

    private static readonly Histogram<long> iHistogram = myFirstMeter.CreateHistogram<long>("MyFirstHistogram");

    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        SetMeterProvider();

       



            iCounter.Add(100, new("name", "apple"), new("color", "red"));

            iCounter.Add(100, new("name", "lemon"), new("color", "yellow"));

            Thread.Sleep(12000);

            iCounter.Add(100, new("name", "apple"), new("color", "red"));


            iHistogram.Record(100, new("name", "apple"), new("color", "red"));
            iHistogram.Record(200, new("name", "lemon"), new("color", "yellow"));
            iHistogram.Record(300, new("name", "lemon"), new("color", "yellow"));
       
        Console.Read();
    }

    static MeterProvider SetMeterProvider()
    {
        var meterProvider = Sdk.CreateMeterProviderBuilder()
             .AddMeter("MyFirstMeter")
             .AddConsoleExporter()
             .AddGenevaMetricExporter(options
             =>
             {
                 options.MetricExportIntervalMilliseconds = 1000;
                 options.ConnectionString = "Account=OTelMonitoringAccount;Namespace=OTelMetricNamespace";
             })
             .Build();
        return meterProvider;
    }

   
}