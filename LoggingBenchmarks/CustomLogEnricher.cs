using Microsoft.R9.Extensions.Enrichment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBenchmarks
{
    public class CustomLogEnricher : ILogEnricher
    {
        public void Enrich(IEnrichmentPropertyBag enrichmentBag)
        {
            throw new NotImplementedException();
        }
    }
}
