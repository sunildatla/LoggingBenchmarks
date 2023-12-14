using System.Diagnostics;

namespace WebAppForLogs
{
    public class CustomContextPropogator : DistributedContextPropagator
    {
        public override IReadOnlyCollection<string> Fields => throw new NotImplementedException();

        public override IEnumerable<KeyValuePair<string, string?>>? ExtractBaggage(object? carrier, PropagatorGetterCallback? getter)
        {
            throw new NotImplementedException();
        }

        public override void ExtractTraceIdAndState(object? carrier, PropagatorGetterCallback? getter, out string? traceId, out string? traceState)
        {
            throw new NotImplementedException();
        }


        public override void Inject(Activity? activity, object? carrier, PropagatorSetterCallback? setter)
        {
            throw new NotImplementedException();
        }
    }
}
