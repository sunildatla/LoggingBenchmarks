using Microsoft.AspNetCore.Http;
using Microsoft.R9.Extensions.Tracing.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R9Samples.Enrichments
{
    public class HttpEnrichment : IHttpTraceEnricher
    {
        public void Enrich(Activity activity, HttpRequest request)
        {
           
            throw new NotImplementedException();
        }
    }
}
