using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonitoringExample.Api.Enums;
using MonitoringExample.Api.Monitoring;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MonitoringExample.Api.Extensions;

namespace MonitoringExample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;
        private readonly MetricsRegistry _metricsRegistry;

        public TestController(ILogger<TestController> logger, MetricsRegistry metricsRegistry)
        {
            _logger = logger;
            _metricsRegistry = metricsRegistry;
        }

        /// <summary>
        /// Generate sample data for use in monitoring
        /// </summary>
        /// <param name="count">Count of requests in each second</param>
        /// <param name="seconds">Total continuous seconds of data generation</param>
        /// <returns></returns>
        [HttpGet("Monitoring/GenerateTestData/{count}/{seconds}")]
        public async Task<IActionResult> GenerateMetricsTestData(int count, int seconds)
        {

            await Task.Yield();

            _ = Parallel.For(0, count, delegate (int requestCount)
            //for (int i = 0; i < count; i++)
            {
                for (int second = 0; second < seconds; second++)
                {
                    var application = ((Client)Extension.RandomGenerator(1, 5));

                    var endpoint = (Endpoints)Extension.RandomGenerator(1, 4);

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
                }
            });
            return Ok();
        }


    }
}
