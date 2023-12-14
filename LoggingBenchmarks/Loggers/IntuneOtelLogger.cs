//using LoggingBenchmarks.Configuration;
//using LoggingBenchmarks.OpenTelemetry;
//using LoggingBenchmarks.Providers.LoggingProvider;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Metadata.Ecma335;
//using System.Text;
//using System.Threading.Tasks;
//using System.Transactions;

//namespace LoggingBenchmarks.Loggers
//{
//    public sealed class IntuneOtelLogger<T> : ILogger<T>
//    {
//        private readonly ILogger _logger;

//        /// <summary>
//        /// Creates a new <see cref="Logger{T}"/>.
//        /// </summary>
//        /// <param name="factory">The factory.</param>
//        public IntuneOtelLogger(ILoggerFactory factory)
//        {
//            if (factory == null) throw new ArgumentNullException(nameof(factory));

//            _logger = factory.CreateLogger(GetCategoryName());
//        }

//        /// <inheritdoc />
//        IDisposable? ILogger.BeginScope<TState>(TState state)
//        {
//            return _logger.BeginScope(state);
//        }

//        /// <inheritdoc />
//        bool ILogger.IsEnabled(LogLevel logLevel)
//        {
//            return _logger.IsEnabled(logLevel);
//        }

//        /// <inheritdoc />
//        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
//        {
//            _logger.Log(logLevel, eventId, state, exception, formatter);
//        }

//        private static string GetCategoryName() => typeof(T).Name;



//        private readonly string _categoryName;

//        private readonly LogLevel _minLevel;

//        //Use LoggerProvider set up at the startup to control the logger behavior at the time of logging.
//        private readonly IntuneOtelLoggerProvider _loggerProvider;

//        private readonly Func<LoggingConfig> _getCurrentConfig;


//        public IntuneOtelLogger(string categoryName, IntuneOtelLoggerProvider loggerProvider, LogLevel logLevel, Func<LoggingConfig> getCurrentCOnfig)
//        {
//            _categoryName = categoryName;
//            _loggerProvider = loggerProvider;
//            _getCurrentConfig = getCurrentCOnfig;
//            _minLevel = logLevel;

//        }

//        //TBD :Scope implementation needs to be added.
//        public IDisposable BeginScope<TState>(TState state)
//        {

//            if (_loggerProvider.IncludeScopes)
//            {

//            }
//            return NullScope.Instance;
//        }

//        public bool IsEnabled(LogLevel level)
//        {
//            return false;
//        }

//        /// <summary>
//        /// logger.loginformation($"My name is {Sunil}")
//        /// logger.loginformation("My name is {0}",var1)

//        /// 
//        /// </summary>
//        /// <typeparam name="TState"></typeparam>
//        /// <param name="logLevel"></param>
//        /// <param name="eventId"></param>
//        /// <param name="state"></param>
//        /// <param name="exception"></param>
//        /// <param name="formatter"></param>
//        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
//        {
//            //return;
//            //LoggingConfig config = _getCurrentConfig();
//            //formatter.Invoke(state, exception);
            
//            if (state is IEnumerable<KeyValuePair<string, object>> logvalues)
//            {
//                logvalues.ToList().Add(new KeyValuePair<string, object>("a", "b"));
//                logvalues.ToList().ForEach(kv => Console.WriteLine(kv.Key + kv.Value));
//            }

//        }
//    }

//}
