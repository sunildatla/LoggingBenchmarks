using System.Diagnostics;

namespace WebAppForLogs
{
    public sealed class SkipHttpClientActivityPropagator : DistributedContextPropagator
    {
        private readonly DistributedContextPropagator _originalPropagator = Current;

        public override IReadOnlyCollection<string> Fields => _originalPropagator.Fields;

        public override void Inject(Activity? activity, object? carrier, PropagatorSetterCallback? setter)
        {
            if (activity?.OperationName == "System.Net.Http.HttpRequestOut")
            {
                activity = activity.Parent;
            }
            

            _originalPropagator.Inject(activity, carrier, setter);
        }

        public override void ExtractTraceIdAndState(object? carrier, PropagatorGetterCallback? getter, out string? traceId, out string? traceState) =>
            _originalPropagator.ExtractTraceIdAndState(carrier, getter, out traceId, out traceState);

        public override IEnumerable<KeyValuePair<string, string?>>? ExtractBaggage(object? carrier, PropagatorGetterCallback? getter) =>
            _originalPropagator.ExtractBaggage(carrier, getter);
    }
}
