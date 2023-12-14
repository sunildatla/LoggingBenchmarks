
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Throttling;
internal class Program
{
    private static void Main(string[] args)
    {


        var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.Development.json", false);

        

        IServiceCollection serviceCollection = new ServiceCollection();
       var logger= serviceCollection.ConfigureIntuneLogging().BuildServiceProvider().GetService<ILoggerFactory>().CreateLogger<Program>();
        
        for (int i = 0; i < 100; i++)
        {

            logger.LogInformation("Hello World");
        }
        
        // .AddOpenTelemetryTracing()
        //.ConfigureIntuneRedaction()
        //.ConfigureIntuneTracing();

    }
}