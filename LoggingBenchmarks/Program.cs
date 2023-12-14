using LoggingBenchmarks.Configuration;
using LoggingBenchmarks.Loggers;
using LoggingBenchmarks.OpenTelemetry;
using LoggingBenchmarks.OpenTelemetry.Extensions;
using Microsoft.CorrelationVector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Management.Services.Diagnostics;
using Microsoft.Management.Services.Diagnostics.UnifiedApi;
using Microsoft.R9.Extensions.AsyncState;
using Microsoft.R9.Extensions.Redaction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics.Eventing;
using System.Threading;

namespace LoggingBenchmarks
{
    public class Program
    {
        //public static ILogger<LoggerCategory> logger;
        public static ILogger logger;
        public static IRedactorProvider redactorProvider;

        private static readonly Meter MyMeter = new("LoggingBenchmarks.MyFirstMeter", "1.0");

        private static readonly Counter<long> MyFruitCounter = MyMeter.CreateCounter<long>("MyFruitCounter");
        private static readonly ActivitySource MyActivitySource = new ActivitySource("LoggingBenchmarks");



        /// <summary>
        /// USAGE: LoggingBenchmarks.exe -f "*OneParamLoggingBenchmarks*"   
        ///        LoggingBenchmarks.exe -p ETW -f "*OneParamLoggingBenchmarks*" 
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {


            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.Development.json", false);

            IConfiguration config = builder.Build();
            LoggingConfig loggingConfig = new LoggingConfig();
            config.GetSection("Logging").Bind(loggingConfig);



            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection
            .AddSingleton(loggingConfig)
            .Configure<LoggingConfig>(config.GetSection("Logging"))
            .AddIntuneTracing()
            .AddIntuneMetrics()
           // .AddOpenTelemetryTracing()
            .ConfigureIntuneLogging();
            //.ConfigureIntuneRedaction()
            //.ConfigureIntuneTracing();


            //ActivitySource MyActivitySource = new ActivitySource("LoggingBenchmarks");

            //Activity.Current = MyActivitySource.StartActivity("Cooking");

            //using (Activity makeADishActivity = Activity.Current)
            //{
            //    makeADishActivity?.AddEvent(new ActivityEvent("Starting the Dish"));

            //    makeADishActivity?.SetTag("Water is", "Hot");

            //    using (Activity addVeggies = MyActivitySource.StartActivity("AddVegetabkes"))
            //    {
            //        Thread.Sleep(1000);

            //        addVeggies?.SetTag("Vegetables", "Carrot Added");
            //        addVeggies?.SetTag("Condiments", "Salt Added");
            //    }

            //    makeADishActivity?.AddEvent(new ActivityEvent("Dish can be served now", DateTime.Now));
            //    Thread.Sleep(1000);
            // }





            //         var asyncState = serviceCollection.BuildServiceProvider().GetRequiredService<IAsyncState>();
            //         asyncState.Initialize();
            //         var asyncContext = serviceCollection.BuildServiceProvider().GetRequiredService<IAsyncContext<CorrelationVector>>();
            //         asyncContext.Set(new CorrelationVector());

          
                    logger = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>().CreateLogger<LoggerCategory>();

            logger.LogInformation("This is my {name}", "Sunil");

            //redactorProvider= serviceCollection.BuildServiceProvider().GetService<IRedactorProvider>();

            	LoggingExamples(serviceCollection);    

            Console.Read();

        }

        private static void LoggingExamples(IServiceCollection serviceCollection)
        {
            OpenTelemetry.UnifiedApi.IntuneLogInitializer.Initialize(null, "", "", "", "", "", "", "", "", serviceCollection);

            LoggerContext logContext = new LoggerContext()
            {
                ActivityId = Guid.NewGuid(),
                RelatedActivityId = Guid.NewGuid(),
                SessionId = Guid.NewGuid(),
                CorrelationVector = CorrelationVectorImplHolder.CorrelationVectorManager.Create(Guid.NewGuid())
            };

            

            #region Logger
            ////Example 1: Using LogInformation extension of logger
           ((ILogger<Program>)logger).LogInformation(new EventId(1), "My name is Sunil", logContext.ActivityId );

            logger.Log(logLevel: LogLevel.Information,
               eventId: default,
               state: new List<KeyValuePair<string, object>>()
               {
                        new KeyValuePair<string, object>("food", "apple"),
                        new KeyValuePair<string, object>("price", 3.99)
               },
               exception: new InvalidOperationException("Oops! Food is spoiled!"),
               formatter: (state, ex) => "Example formatted message."
       );

            ////Example 2: Using LogInformation extension of logger with structured logging 
            //logger.LogInformation(new EventId(2), "My name is {FirstName}", "Sunil");

            //  Example 3: Using Scopes
            using (logger.BeginScope(new Dictionary<string, object> { { "activityId", Guid.NewGuid() } }))
            {
                using (logger.BeginScope(new Dictionary<string, object> { { "UserId", Guid.NewGuid() } }))
                {
                    logger.LogInformation(new EventId(3), "My name is {0}{1}", "Sunil", "Datla");
                }
            }


            ////Example 4: Using Log Method with State and logging exception
       
            #endregion

            //#region UnifiedApiLogger
            ///*Fwd Compatibility*/

            //////Example 5: IntuneLogger without Context
            ////StaticIntuneLogger.LogEventWithR9FastLogger("ComponentNameWithoutContext", "NewEvent1", 1, new KeyValuePair<string, string>("Key1","Value1"));
            //////Example 6: IntuneLogger with Context
            //StaticIntuneLogger.LogEventWithR9Logger(logContext, "ComponentNameWithContext", "NewEvent2", 1, new KeyValuePair<string, string>("Key2", "Value2"));
            //#endregion

            //#region FastLogger
            //////Example 7: Direct Fast Logger
            //IntuneFastLogger.LogEvent(logger, LogLevel.Information, logContext, new LogEvent() { ComponentName = "FastLoggingComponent", EventUniqueName = "FastLoggerUniqueName", ThreadId = 100 }, "thiisunil");
            //#endregion

            //#region SecurityLogger
            //////Example 8: Redaction Fast Logger
            ////IntuneFastLogger.LogEventWithRedactorEUII(logger, redactorProvider, LogLevel.Information, logContext, new LogEvent() { ComponentName = "FastLoggingComponentEUII", EventUniqueName = "FastLoggerUniqueName", ThreadId = 100 }, "thiisunil");
            //#endregion
        }

    }
}


#region commentedcode

//ActivitySource source = new ActivitySource("Microsoft.Aspnet.Core");
// Console.WriteLine("Started Trace");
//  Activity sourceActivity = source.CreateActivity("FirstActivity",ActivityKind.Server);
//sourceActivity.Start();
//sourceActivity.AddTag("FirstTag", "FirstValue");
//  sourceActivity.AddTag("ApplicationName", "FirstApplication");
//sourceActivity.AddBaggage("FirstBaggage", "FirstBagValue");
//  sourceActivity.Stop();

//Activity sourceActivity2 = new Activity("operationname");
//         sourceActivity2.Start();
//         sourceActivity2.AddTag("FirstTag", "FirstValue");
//         sourceActivity2.AddTag("ApplicationName", "FirstApplication");
//         sourceActivity2.AddBaggage("FirstBaggage", "FirstBagValue");
//         sourceActivity2.Stop();
//         Console.WriteLine("Ended Trace");


//         InstrumentationWithActivitySource activitysourcehttptest = new InstrumentationWithActivitySource();
//activitysourcehttptest.Start();



////         for (int i = 0; i < 2; i++)
////{
////	await TraceProvider.Execute("MyFirstActivity", new Dictionary<string, object> { { "foo", i }, { "bar", "Hello, World!" }, { "baz", new int[] { 1, 2, 3 } }, { "BuildVersion", "2022.10.08" } });
////}
//  MeterLogger.ExecuteTest();

//////Logging

//         OptimizedLogger _oLogger = new OptimizedLogger(logger);

//Employee emp = new Employee() { Id = 1, Name = "Sumnil" };

//         _oLogger.LogComplex(emp);
////OneParamLoggingBenchmarks loggingBenchmarks = new OneParamLoggingBenchmarks();

////         loggingBenchmarks.Setup(serviceCollection);
////         loggingBenchmarks.Start();

////loggingBenchmarks.StandardLoggingWithScope();
////loggingBenchmarks.StandardLoggingOneParam();
////loggingBenchmarks.OptimisedLoggingOneParam();
////loggingBenchmarks.CompileTimeLoggingOneParam();
////loggingBenchmarks.StandardLoggingWithLog();

#endregion