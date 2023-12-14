using OpenTelemetry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.Processors
{
    /// <summary>
    /// This will be called whenever an Activity is written to enrich activity with additional tags
    /// </summary>
    internal class TraceCommonTagProcessor : BaseProcessor<Activity>
    {
        private readonly string name;
        public TraceCommonTagProcessor(string name = "BasicProcessor")
        {
            this.name = name;
        }
     
        public override void OnStart(Activity data)
        {
            data?.SetTag("MachineName", "SunilDtlaDKTP");
            data?.SetTag("ServiceName", Assembly.GetExecutingAssembly().FullName);
            base.OnStart(data);
     
        }

        public override void OnEnd(Activity data)
        {
           // Console.WriteLine($"{this.name}.OnEnd({data.DisplayName})");
        }

        protected override bool OnForceFlush(int timeoutMilliseconds)
        {
            Console.WriteLine($"{this.name}.OnForceFlush({timeoutMilliseconds})");
            return true;
        }
        protected override bool OnShutdown(int timeoutMilliseconds)
        {
            Console.WriteLine($"{this.name}.OnShutdown({timeoutMilliseconds})");
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            Console.WriteLine($"{this.name}.Dispose({disposing})");
        }
    }
}
