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
    internal class TraceFilteringProcessor : CompositeProcessor<Activity>
    {
        private readonly Func<Activity, bool> filter;  
        public TraceFilteringProcessor(BaseProcessor<Activity> processor ,Func<Activity,bool> filter) : base(new[] { processor })
        {
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        public override void OnEnd(Activity data)
        {
            
            //Call the underlying processor
            //if the filter returns true.
            if (this.filter(data))
                base.OnEnd(data);   
        }
    }
}
