using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace R9Samples
{
    internal static partial class LoggerMessageLogger
    {
        [Benchmark(Baseline = true)]
        [LoggerMessage(1, LogLevel.Information, "Learn more about \"{topic}\" at \"{docLocation}\"")]
        public static partial void LearnMoreAt(this ILogger logger, string topic, string docLocation);

    }
}
