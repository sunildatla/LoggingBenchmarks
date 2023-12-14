using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Data.Classification;
using Microsoft.R9.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace R9Samples
{
  
    public static partial class FastLogger
    {
        [LogMethod(1, LogLevel.Information, "Learn more about \"{topic}\" at \"{docLocation}\"")]
        public static partial void LearnMoreAt(this ILogger logger, string topic, string docLocation);

       
        [LoggerMessage(1, LogLevel.Information, "Learn more about \"{topic}\" at \"{docLocation}\"")]
        public static partial void LearnMoreAtBase(this ILogger logger,[EUII] string topic, string docLocation);

        //[Benchmark]
        //[LogMethod(2, LogLevel.Error, "An exception of type ArgumentNullException was thrown")]
        //public static partial void LogArgumentNullException(this ILogger logger, Exception exception);
    }

  
}
