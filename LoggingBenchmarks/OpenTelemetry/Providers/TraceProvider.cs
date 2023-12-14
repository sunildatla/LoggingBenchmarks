using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace LoggingBenchmarks.Providers
{
    internal static class TraceProvider
    {
        private static readonly ActivitySource MyActivitySource = new ActivitySource("LoggingBenchmarks");



        public static async Task Execute(string activityName, Dictionary<string, object> tags)
        {
            
            Activity.Current = MyActivitySource.StartActivity(activityName);

            using (Activity activity = Activity.Current)
            {

                foreach (var item in tags)
                {
                    activity?.SetTag(item.Key, item.Value);
                }

                await StepOne();
                activity?.AddEvent(new ActivityEvent("Part way there"));
                await StepTwo();
                activity?.AddEvent(new ActivityEvent("Done now"));
            }
        }

        private static async Task StepTwo()
        {
            using (Activity activity = MyActivitySource.StartActivity("StepTwo"))
            {
                await Task.Delay(500);
            }
        }

        private static async Task StepOne()
        {
            using (Activity activity = MyActivitySource.StartActivity("StepOne"))
            {
                await Task.Delay(500);
            }
        }
    }
}
