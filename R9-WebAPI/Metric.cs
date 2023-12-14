using Microsoft.R9.Extensions.Metering;

namespace R9_WebAPI
{
    internal static partial class Metric
    {

        [Histogram]
        public static partial Latency CreateLatency(IMeter meter);

        [Gauge]
        public static partial MemoryUsage CreateMemoryUsage(IMeter meter);

      
    }
}
