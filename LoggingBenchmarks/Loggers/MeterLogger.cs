using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Metrics;
namespace LoggingBenchmarks.Loggers
{
    public static class MeterLogger
    {
        private static readonly string MetricName = "LoggingBenchmarks.MyFirstMeter";
        private static readonly Meter MyMeter;

        private static readonly Counter<long> MyFruitCounter;
        private static readonly Histogram<long> RequestCount;

        static MeterLogger()
        {
            MyMeter = new Meter(MetricName);
            MyFruitCounter = MyMeter.CreateCounter<long>("MyFruitCounter");
            RequestCount = MyMeter.CreateHistogram<long>(MetricName, "RequestId");

            
        }

        public static void ExecuteTest()
        {
            CreateMetric(new KeyValuePair<string, object>[] { new("Color", "blue"), new("Party", "Democrat") }, 1);
            CreateMetric(new KeyValuePair<string, object>[] { new("Color", "Red"), new("Party", "Republican") }, 5);
            CreateMetric(new KeyValuePair<string, object>[] { new("Color", "blue"), new("Party", "Democrat") }, 10);
            CreateMetric(new KeyValuePair<string, object>[] { new("Color", "Red"), new("Party", "Independant") }, 1);
            CreateMetric(new KeyValuePair<string, object>[] { new("Color", "Green"), new("Party", "Independant") }, 1);
            CreateMetric(new KeyValuePair<string, object>[] { new("Color", "White"), new("Party", "NA") }, 1);


            CreateHistogram(10, new KeyValuePair<string, object>[] { new("verb", "GET"), new("status", "200") });
            CreateHistogram(20, new KeyValuePair<string, object>[] { new("verb", "POST"), new("status", "200") });
            CreateHistogram(5, new KeyValuePair<string, object>[] { new("verb", "GET"), new("status", "401") });
            CreateHistogram(80, new KeyValuePair<string, object>[] { new("verb", "GET"), new("status", "503") });
            CreateHistogram(10, new KeyValuePair<string, object>[] { new("verb", "POST"), new("status", "401") });
            CreateHistogram(30, new KeyValuePair<string, object>[] { new("verb", "POST"), new("status", "202") });
            CreateHistogram(10, new KeyValuePair<string, object>[] { new("verb", "POST"), new("status", "200") });


            
            //Asyncrhonous Counter
            MyMeter.CreateObservableCounter("ProcessCpu", () => GetThreadCpuTime(Process.GetCurrentProcess()), "ms");

            //Asyncrhnous Gauge
            MyMeter.CreateObservableGauge("Thread.State", () => GetThreadState(Process.GetCurrentProcess()));


        }

        private static IEnumerable<Measurement<int>> GetThreadState(Process process)
        {

            foreach (ProcessThread t in process.Threads)
            {
                yield return new((int)t.ThreadState, new KeyValuePair<string, object>[] { new("ProcessId",process.Id),new("ThreadId",t.Id)});
            }
        }

        private static IEnumerable<Measurement<double>> GetThreadCpuTime(Process process)
        {
            foreach(ProcessThread t in process.Threads)
            {
                yield return new(t.TotalProcessorTime.Milliseconds, new("ProcessId", process.Id), new("ThreadId", t.Id));
            }
        }

        public static void CreateMetric(KeyValuePair<string, object>[] dimensions, long counter)
        {
            MyFruitCounter.Add(counter, dimensions);
            
        }

        public static void CreateHistogram(long value, KeyValuePair<string, object>[] measurements)
        {
            RequestCount.Record(value, measurements);
        }
    }
}
