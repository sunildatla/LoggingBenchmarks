using BenchmarkDotNet.Attributes;
using LoggingBenchmarks.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LoggingBenchmarks
{
    [MemoryDiagnoser]
	public class OneParamLoggingBenchmarks
	{
		private StandardLogger _sut1;
		private OptimizedLogger _sut2;
		private LoggerMessageLogger _sut3;
		private ILogger logger;

		private const string Value1 = "Value";

        [GlobalSetup]
        public void Start()
		{
        }

		public void Setup(IServiceCollection serviceCollection)
		{
			var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();

			logger= loggerFactory.CreateLogger("LoggingBenchmarks");

            _sut1 = new StandardLogger(logger);
            _sut2 = new OptimizedLogger(logger);
            _sut3 = new LoggerMessageLogger(logger);
			
        }

		[Benchmark(Baseline = true)]
		public void StandardLoggingOneParam() => _sut1.LogOnceWithOneParam(Value1);

		[Benchmark]
		public void StandardLoggingWithLog() => _sut1.Log("This is a formatted message",LogLevel.Information);

		[Benchmark]
		public void StandardLoggingWithScope() => _sut1.LogWithScope("This is custom message");

		[Benchmark]
		public void OptimisedLoggingOneParam() => _sut2.Log(Value1);

		[Benchmark]
		public void CompileTimeLoggingOneParam() => _sut3.CouldNotOpenSocketOneParam("SunilDatlaDKSTP");
	}
}
