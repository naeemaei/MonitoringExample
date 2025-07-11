using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;

namespace MonitoringExample.Api.Monitoring
{
    public class MetricsRegistry(IOptionsMonitor<MetricConfig> metricConfig, ILogger<MetricsRegistry> logger)
    {

        private static Counter RequestCounter = Metrics.CreateCounter("application_request_counter", "Request Counter", configuration: new CounterConfiguration
        {
            LabelNames = ["response_code", "client", "endpoint", "service"]
        });
        private static Gauge RequestDuration = Metrics.CreateGauge("application_request_duration", "Request Duration", configuration: new GaugeConfiguration
        {
            LabelNames = ["response_code", "client", "endpoint", "service"]
        });

        public void IncreaseRequestCounter(int responseCode, string client, string endpoint, string service)
        {
            if (metricConfig.CurrentValue.Enabled)
            {
                RequestCounter.WithLabels(responseCode.ToString(), client, endpoint, service).Inc();
            }
        }

        public void SetRequestDuration(int responseCode, string client, string endpoint, string service, double duration)
        {
            if (metricConfig.CurrentValue.Enabled)
            {
                RequestDuration.WithLabels(responseCode.ToString(), client, endpoint, service).Set(duration);
            }
        }

    }
}
