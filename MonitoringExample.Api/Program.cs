using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MonitoringExample.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseMetricsWebTracking()
                    .UseMetrics(option =>
                    {

                        option.EndpointOptions = endpointOptions =>
                        {
                            endpointOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                            endpointOptions.EnvironmentInfoEndpointEnabled = false;
                        };
                    });
                });
    }
}
