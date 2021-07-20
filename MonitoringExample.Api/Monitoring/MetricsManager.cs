using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using Microsoft.Extensions.Logging;
using MonitoringExample.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitoringExample.Api.Monitoring
{
    public class MetricsManager : IMetricsManager
    {
        private readonly IMetrics _metrics;
        private readonly ILogger<MetricsManager> _logger;

        public MetricsManager(IMetrics metrics, ILogger<MetricsManager> logger)
        {
            _metrics = metrics;
            _logger = logger;
        }

        public void Increment(CounterOptions options, int? responseCode, Dictionary<string, object> tags)
        {
            try
            {
                if (responseCode.HasValue && tags.Any())
                {
                    var labels = MetricTags.Empty;
                    foreach (var item in tags.Where(e => e.Value != null).ToList())
                    {
                        labels = labels.Add(item.Key, item.Value ?? "");
                    }

                    _metrics.Measure.Counter.Increment(options, labels.Add(nameof(responseCode), responseCode.Value));
                }
                else
                {
                    _logger.LogError($"{options.Name} => Warning in Metric Increment: {tags.ToJson()}");
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"{options.Name} => Exception in Metric Increment: {ex.Message} *** {ex.InnerException?.Message}");
            }
        }

        public void SetValue(GaugeOptions options, Dictionary<string, object> tags, double? value)
        {
            try
            {
                if (tags.Any())
                {
                    var labels = MetricTags.Empty;
                    foreach (var item in tags.Where(e => e.Value != null).ToList())
                    {
                        labels = labels.Add(item.Key, item.Value ?? "");
                    }

                    _metrics.Measure.Gauge.SetValue(options, labels, value ?? 0);
                }
                else
                {
                    _logger.LogError($"{options.Name} => Warning in Metric SetValue: {tags.ToJson()}");
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"{options.Name} => Exception in Metric SetValue: {ex.Message} *** {ex.InnerException?.Message}");
            }

        }
    }
}
