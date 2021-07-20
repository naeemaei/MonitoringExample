using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MonitoringExample.Api.Monitoring;

namespace MonitoringExample.Api.Jobs
{
    public class GenerateMonitoringValuesJob : HostedService
    {
        protected override TimeSpan Interval => TimeSpan.FromMinutes(5);
        protected override string JobName => nameof(GenerateMonitoringValuesJob);


        public GenerateMonitoringValuesJob(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory) : base(serviceProvider, serviceScopeFactory)
        {

        }

        protected override async Task OnDoWork(IServiceProvider serviceProvider)
        {
            var dataGenerator = serviceProvider.GetService<IMetricDataGenerator>();
            dataGenerator.GenerateMetricsTestData(10, 298);
            await Task.Yield();
        }

        protected override async Task OnStopWork(IServiceProvider serviceProvider)
        {
            await Task.Yield();
        }
    }
}