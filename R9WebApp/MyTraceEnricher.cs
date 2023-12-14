using Microsoft.R9.Extensions.Enrichment;
using System.Diagnostics;

namespace R9WebApp
{
    public class MyTraceEnricher : ITraceEnricher
    {
        public void Enrich(Activity activity)
        {
            activity.SetTag("CustomKey", "CustomValue");
        }
    }
}
