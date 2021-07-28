using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonitoringExample.Api.Enums;
using MonitoringExample.Api.Extensions;

namespace MonitoringExample.Api.Monitoring
{
    public interface IMetricDataGenerator
    {
        void GenerateMetricsTestData(int count, int seconds);
    }

    public class MetricTestDataGenerator : IMetricDataGenerator
    {
        private readonly MetricsRegistry _metricsRegistry;
        private readonly ILogger<MetricTestDataGenerator> _logger;

        public MetricTestDataGenerator(MetricsRegistry metricsRegistry, ILogger<MetricTestDataGenerator> logger)
        {
            _metricsRegistry = metricsRegistry;
            _logger = logger;
        }
        public void GenerateMetricsTestData(int count, int seconds)
        {
            var temp1 = Extension.RandomGenerator(Constants.MinNum, Constants.MidNum);
            var temp2 = Extension.RandomGenerator(Constants.MinNum, Constants.MidNum);

            if (temp1 + temp2 > 70)
                count = Convert.ToInt32(count * 1.3);
            else if (temp1 + temp2 > 50)
                count = Convert.ToInt32(count * 0.7);

            _ = Parallel.For(0, count, delegate (int requestCount)
            //for (int i = 0; i < count; i++)
            {
                for (int second = 0; second < seconds; second++)
                {
                    _logger.LogInformation($"Start of second: {second}, iterate: {requestCount}");
                    var application = ((Client)Extension.RandomGenerator(1, 5));

                    var endpoint = Extension.GenerateRandomEndpoint();

                    var service = Extension.GenerateRandomExternalService(requestCount);

                    var statusCode = Extension.GenerateRandomStatusCode(requestCount, service);

                    int responseTime = Extension.GenerateRandomResponseTime();

                    Dictionary<string, object> labels = new()
                    {
                        { nameof(application), application.GetDescription() },
                        { nameof(endpoint), endpoint.GetDescription() },
                        { nameof(service), service.GetDescription() },
                    };

                    Task.Run(() => _metricsRegistry.RegisterMetrics((int)statusCode, responseTime, labels).ConfigureAwait(false));

                    Thread.Sleep(1000);
                    _logger.LogInformation($"End of second: {second}, iterate: {requestCount}");

                }
            });
        }
    }
}