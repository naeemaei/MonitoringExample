using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MonitoringExample.Api.Monitoring;

namespace MonitoringExample.Api.Jobs
{
    public class GenerateMonitoringValuesJob(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory) : HostedService(serviceProvider, serviceScopeFactory)
    {
        protected override TimeSpan Interval => TimeSpan.FromSeconds(30);
        protected override string JobName => nameof(GenerateMonitoringValuesJob);

        protected override async Task OnDoWork(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetRequiredService<IOptionsMonitor<MetricConfig>>();
            var dataGenerator = serviceProvider.GetService<IMetricDataGenerator>();
            dataGenerator.GenerateMetricsTestData(config.CurrentValue.MetricPerSecond, 28);
            await Task.Yield();
        }

        protected override async Task OnStopWork(IServiceProvider serviceProvider)
        {
            await Task.Yield();
        }
    }
}