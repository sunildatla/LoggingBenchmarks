using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.Constants
{
    internal static class ContextVariables
    {
        public static List<string> contextPropValues = new List<string>();
        static ContextVariables()
        {
            typeof(ContextVariables)
                .GetFields()
                .ToList()
                .ForEach(p => contextPropValues.Add(p.GetValue(null).ToString()));
        }
        public const string ActivityId = "ActivityId";
        public const string RelatedActivityId = "RelatedActivityId";
        public const string SessionId = "SessionId";
        public const string UserId = "UserId";
        public const string DeviceId = "DeviceId";
        public const string AccountId = "AccountId";
        public const string ComponentName = "ComponentName";
        public const string CV = "cV";
        public const string CorrelationVector = "CorrelationVector";
    }
}
