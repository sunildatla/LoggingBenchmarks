using LoggingBenchmarks.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Management.Services.Diagnostics.UnifiedApi.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks.OpenTelemetry.UnifiedApi
{
    public static  partial class IntuneLogInitializer
    {
        public static bool isOtelLoggingEnabled = false;

        //This logger is used for OTEL fast logging.
        public static Microsoft.Extensions.Logging.ILogger<LoggerCategory> _logger;
        public static void Initialize(ILogProviders logProvidersCollection, string mdsNamespace, string monitoringTenant, string monitoringRole, string monitoringRoleInstance, string mdmMonitoringAccount, string serviceName, string applicationName, string buildVersion)
        {
            Initialize(
                      logProvidersCollection: logProvidersCollection,
                      mdsNamespace: mdsNamespace,
                      monitoringTenant: monitoringTenant,
                      monitoringRole: monitoringRole,
                      monitoringRoleInstance: monitoringRoleInstance,
                      serviceName: serviceName,
                      applicationName: applicationName,
                      buildVersion: buildVersion,
                      null
                      );
        }

        public static void Initialize(ILogProviders logProvidersCollection, string mdsNamespace, string monitoringTenant, string monitoringRole, string monitoringRoleInstance, string mdmMonitoringAccount, string serviceName, string applicationName, string buildVersion,IServiceCollection serviceCollection)
        {
            Initialize(
                      logProvidersCollection: logProvidersCollection,
                      mdsNamespace: mdsNamespace,
                      monitoringTenant: monitoringTenant,
                      monitoringRole: monitoringRole,
                      monitoringRoleInstance: monitoringRoleInstance,
                      serviceName: serviceName,
                      applicationName: applicationName,
                      buildVersion: buildVersion,
                      serviceCollection
                      );
        }



        private static void Initialize(ILogProviders logProvidersCollection, string mdsNamespace, string monitoringTenant, string monitoringRole, string monitoringRoleInstance, string serviceName, string applicationName, string buildVersion,IServiceCollection serviceCollection)
        {
            if(serviceCollection is not null)
            {
                //The below ILogger initialization may change or get new additions based on the extensions we add to support config , logproviders ,etc
                _logger = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>().
                    CreateLogger<LoggerCategory>();
                //Feature flag to disable 
                isOtelLoggingEnabled = true;
            }
            //throw new NotImplementedException();
        }
    }
}
