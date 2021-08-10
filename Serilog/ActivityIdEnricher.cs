﻿using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace Serilog_Sample
{
    class ActivityIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "ActivityId", Trace.CorrelationManager.ActivityId));
        }
    }
}
