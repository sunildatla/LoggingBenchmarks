using Microsoft.R9.Extensions.Redaction;
using System;

namespace LoggingBenchmarks.OpenTelemetry
{
    internal class YellingRedactor : BaseRedactor
    {
        public override int GetRedactedLength(ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
        {
            throw new NotImplementedException();
        }
    }
}