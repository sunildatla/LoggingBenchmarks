using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks
{
    public class LoggerServiceLocator
    {
        public static LoggerServiceLocator Instance;

        public static IServiceCollection _serviceCollection;

        public static ILoggerFactory _loggerFactory;

        public LoggerServiceLocator(IServiceCollection serviceCollection) {
            if (serviceCollection == null)
                 throw new ArgumentNullException();
                
            _serviceCollection=serviceCollection; 

            _loggerFactory= _serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
            
        }
        
        public static LoggerServiceLocator GetInstance(IServiceCollection serviceCollection)
        {
            if(Instance == null)
                Instance = new LoggerServiceLocator(serviceCollection);
            return Instance;
        }

        public ILogger GetLogger(string name)
        {
            return _loggerFactory.CreateLogger(name);
        }
        public ILogger<T> GetLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }

    }
}
