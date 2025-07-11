using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonitoringExample.Api.Enums;
using MonitoringExample.Api.Monitoring;

namespace MonitoringExample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController(ILogger<TestController> logger, MetricsRegistry metricsRegistry) : ControllerBase
    {
        [HttpGet("search")]
        public IActionResult Search([FromQuery] Client application, [FromQuery] bool getFromExternalService)
        {
            return Ok(true);
        }

        [HttpGet("product-detail-page")]
        public IActionResult ProductDetailPage([FromQuery] Client application, [FromQuery] bool getFromExternalService)
        {
            return Ok(true);
        }

        [HttpGet("register-order")]
        public IActionResult Ordering([FromQuery] Client application, [FromQuery] bool getFromExternalService)
        {
            return Ok(true);
        }

        [HttpGet("register-payment")]
        public IActionResult Payment([FromQuery] Client application, [FromQuery] bool getFromExternalService)
        {
            return Ok(true);
        }

    }
}
