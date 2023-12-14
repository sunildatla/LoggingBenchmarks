//using LoggingBenchmarks.Configuration;
//using LoggingBenchmarks.Loggers;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LoggingBenchmarks.Providers.LoggingProvider
//{
//    /// <summary>
//    /// To create loggers per category
//    /// </summary>
//    public class IntuneOtelLoggerProvider : ILoggerProvider
//    {

//        private LoggingConfig _currentConfig;

//        public bool IncludeScopes { get; set; }

//        private readonly ConcurrentDictionary<string, IntuneOtelLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

//        public IntuneOtelLoggerProvider(IOptionsMonitor<LoggingConfig> currentConfig)
//        {
//            _currentConfig = currentConfig.CurrentValue;
//            this.IncludeScopes = _currentConfig.IncludeScopes;

//        }

//        public ILogger CreateLogger(string categoryName)
//        {
//            return _loggers.GetOrAdd("LoggingBenchmarks.Loggers.LoggerCategory", name => new IntuneOtelLogger("LoggingBenchmarks.Loggers.LoggerCategory", this, LogLevel.Information, GetCurrentConfig));
//        }


//        private LoggingConfig GetCurrentConfig() => _currentConfig;
//        public void Dispose()
//        {
//            _loggers.Clear();
//        }
//    }
//}
