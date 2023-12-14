//using BenchmarkDotNet.Attributes;
//using LoggingBenchmarks.Loggers;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace LoggingBenchmarks
//{
//    [MemoryDiagnoser]
//   // [EtwProfiler]
//	public class NoParamLoggingBenchmarks
//	{
//		private StandardLogger _sut1;
//		private OptimizedLogger _sut2;
//		private LoggerMessageLogger _sut3;

//		[GlobalSetup]
//        public void Setup()
//        {

//        }

//        public void Setup(IServiceCollection serviceCollection)
//		{
		  
//			var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();

//			var logger = loggerFactory.CreateLogger("TEST");
		   

//			_sut1 = new StandardLogger(logger);
//			_sut2 = new OptimizedLogger(logger);
//			_sut3 = new LoggerMessageLogger(logger);
//		}

//		[Benchmark(Baseline = true)]
//		public void StandardLoggingNoParams() => _sut1.LogOnceWithNoParam();

//		[Benchmark]
//		public void OptimisedLoggingNoParams() => _sut2.LogOnceNoParams();


//		[Benchmark]
//		public void CompileTimeLoggingOneParam() => _sut3.CouldNotOpenSocketTwoParam("SunilDatlaDKSTP","8080");
//	}
//}
