using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace serilog_playground;

class ActivityIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "ActivityId", Trace.CorrelationManager.ActivityId));
    }
}