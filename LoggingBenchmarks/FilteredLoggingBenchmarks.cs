//using BenchmarkDotNet.Attributes;
//using LoggingBenchmarks.Loggers;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using OpenTelemetry;
//using OpenTelemetry.Logs;
//using OpenTelemetry.Metrics;
//using OpenTelemetry.Trace;

//namespace LoggingBenchmarks
//{
//    [MemoryDiagnoser]
//  // [EtwProfiler]
//	public class FilteredLoggingBenchmarks
//	{
//		private StandardLogger _sut1;
//		private OptimizedLogger _sut2;

//		private const string Value1 = "Value";
//		private const int Value2 = 1000;

//        [GlobalSetup]
//        public void Setup()
//		{

//		}
		
//		public void Setup(IServiceCollection serviceCollection)
//		{
			

//			var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();

//			var logger = loggerFactory.CreateLogger("TEST");
//			using var meterProvider = Sdk.CreateMeterProviderBuilder()
//									 .AddMeter("LoggingBenchmarks.MyFirstMeter")
//									 .AddConsoleExporter()
//									 .Build();
//			_sut1 = new StandardLogger(logger);
//			_sut2 = new OptimizedLogger(logger);
//		}

//		[Benchmark(Baseline = true)]
//		public void StandardLoggingDebug() => _sut1.LogDebugOnceWithTwoParams(Value1, Value2);

//		[Benchmark]
//		public void LogDebugWithoutLevelCheck() => _sut2.LogOnceUsingLoggerMessageWithoutLevelCheck(Value1, Value2);

//		[Benchmark]
//		public void LogDebugWithLevelCheck() => _sut2.LogOnceUsingLoggerMessageWithLevelCheck(Value1, Value2);
//	}
//}
