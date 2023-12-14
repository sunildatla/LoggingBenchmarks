using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Data.Classification;
using Microsoft.R9.Extensions.Logging;
using Microsoft.R9.Extensions.Redaction;

namespace R9WebApp
{
    public static partial class FastLog
    {
        [LogMethod(1, LogLevel.Information, "User {username} has now {status} status")]
        public static partial void UserAvailabilityChanged(ILogger logger, IRedactorProvider redactorProvider, [EUPI] string username, string status);


        [LogMethod(2, LogLevel.Information, "No params here...")]
        public static partial void ProductSent(ILogger logger, [LogProperties] ProductEntity product);
    }
}
