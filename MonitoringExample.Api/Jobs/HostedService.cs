using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringExample.Api.Jobs
{
    public abstract class HostedService : IHostedService, IDisposable
    {
        private string UniqueId { get; set; } = Guid.NewGuid().ToString();
        protected abstract string JobName { get; }
        private Timer Timer { get; set; }
        protected abstract TimeSpan Interval { get; }
        protected abstract Task OnDoWork(IServiceProvider serviceProvider);
        protected abstract Task OnStopWork(IServiceProvider serviceProvider);
        protected virtual TimeSpan StartTimeOfDay { get; } = TimeSpan.Zero;
        protected ILogger Logger { get; }
        public IServiceScopeFactory ServiceScopeFactory { get; }

        public HostedService(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            Logger = serviceProvider.GetService<ILoggerProvider>().CreateLogger(GetType().FullName);
            ServiceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                TimeSpan timeToGo = StartTimeOfDay;

                Timer = new Timer(DoWork, cancellationToken, timeToGo, Interval);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                var serviceScope = ServiceScopeFactory.CreateScope();
                await OnStopWork(serviceScope.ServiceProvider);
                Timer?.Change(Timeout.Infinite, 0);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }

        private async void DoWork(object state)
        {
            try
            {
                UniqueId = Guid.NewGuid().ToString();
                var key = $"{AppDomain.CurrentDomain.FriendlyName}.{GetType().Name}";
                var expiry = TimeSpan.FromSeconds(1);
                var startTime = DateTime.Now;
                Logger.LogInformation($"Jobs - Start of {JobName}:{UniqueId} at {startTime:yyyy/MM/dd HH:mm:ss.fff} on  {AppDomain.CurrentDomain.FriendlyName}.{Environment.ProcessId}.");

                var serviceScope = ServiceScopeFactory.CreateScope();
                await OnDoWork(serviceScope.ServiceProvider);

                Logger.LogInformation($"Jobs - End of {JobName}:{UniqueId} in {(DateTime.Now - startTime).TotalSeconds} seconds at {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} on  {AppDomain.CurrentDomain.FriendlyName}.{Environment.ProcessId}.");

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        public void TimerTest()
        {
            var result = Timer;
        }

    }
}