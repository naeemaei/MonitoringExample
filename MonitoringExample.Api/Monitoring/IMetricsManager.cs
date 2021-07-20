using App.Metrics.Counter;
using App.Metrics.Gauge;
using System.Collections.Generic;

namespace MonitoringExample.Api.Monitoring
{
    public interface IMetricsManager
    {
        void Increment(CounterOptions options, int? responseCode, Dictionary<string, object> tags);

        void SetValue(GaugeOptions options, Dictionary<string, object> tags, double? value);

    }
}