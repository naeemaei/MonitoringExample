using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringExample.Api.Monitoring
{
    public class MetricsRegistry
    {
        private readonly ILogger<MetricsRegistry> _logger;

        private IConfiguration _configuration { get; }
        private IMetricsManager _metricsManager { get; }
        private ICodeService _codeService { get; }

        private bool RegisterMetricsStatus { get; }

        public MetricsRegistry(IConfiguration configuration, ILogger<MetricsRegistry> logger, IMetricsManager metricsManager, ICodeService codeService)
        {
            _configuration = configuration;
            _logger = logger;
            _metricsManager = metricsManager;
            _codeService = codeService;
            RegisterMetricsStatus = _configuration.GetValue<bool>("RegisterMetricsStatus");
        }


        public static CounterOptions RequestCounter => new()
        {
            Name = "Request Counter",
            MeasurementUnit = Unit.None
        };

        public static GaugeOptions RequestDuration => new()
        {
            Name = "Request Duration",
            MeasurementUnit = Unit.Calls
        };

        public async Task RegisterMetrics(int responseCode, long responseTime, Dictionary<string, object> labels)
        {
            if (RegisterMetricsStatus)
            {
                await _codeService.ExecuteAsync(async () =>
                {
                    await Task.Yield();
                    _metricsManager.Increment(RequestCounter, responseCode, labels);
                    _metricsManager.SetValue(RequestDuration, labels, responseTime);
                }, edi =>
                {
                    _logger.LogError($"Exception in register metrics: {edi?.SourceException?.Message} *** {edi?.SourceException?.StackTrace}");
                    return Task.CompletedTask;
                });
            }
        }

    }
}
