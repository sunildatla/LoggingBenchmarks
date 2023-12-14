using Microsoft.Diagnostics.Tracing.Extensions;
using Microsoft.R9.Extensions.Metering;
using System.ComponentModel.DataAnnotations;

namespace R9WebApp
{
   
    internal static partial class FastMetrics
    {
      
        [Histogram]
        public static partial Latency CreateLatency(IMeter meter);

        
        [Gauge("EnvironmentName","Region","MachineName","ProcessName")]
        public static partial MemoryUsage CreateMemoryUsage(IMeter meter);

        [Counter("EnvironmentName","Region2","RequestName","RequestStatus")]
        public static partial Requests CreateRequests(IMeter meter);

        [Counter(typeof(MetricDimensions))]
        public static partial TotalCount CreateTotalCount(IMeter meter);



    }

    //R9Meter- Defining a Static Meter Provider class which contains one instance of IMeterProvider initialized at startup and used across the application as needed. This pattern avoids adding IMeterProvider as one of the parameter in constructor and resolving using DI , but this need not be used always .
    public static class StaticMeterProvider 
    {
        
        public static IMeterProvider meterProvider { get; set; }

    }
}
