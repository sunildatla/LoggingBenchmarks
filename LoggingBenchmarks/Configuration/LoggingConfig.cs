using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.Configuration
{
    public sealed class LoggingConfig
    {
        public bool Enabled { get; set; } = true;
        public bool IncludeScopes { get; set; } = true;
        public Dictionary<string,string> LogLevel { get; set; }

      
    }
}
