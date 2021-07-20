using Microsoft.AspNetCore.Mvc;
using MonitoringExample.Api.Dtos;
using System.Threading.Tasks;
using PersianTools.Core;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;

namespace MonitoringExample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifyController : ControllerBase
    {
        private readonly ILogger<NotifyController> _logger;
        public NotifyController(ILogger<NotifyController> logger)
        {
            _logger = logger;

        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotify(MonitoringAlertDto request)
        {
            await Task.Yield();
            var messages = request.Alerts.Select(e => e.Annotations.Description);
            foreach (var item in request.Alerts)
            {
                var startDate = item.StartsAt.ToShamsiDateTime().ToString("yyyy/MM/dd");
                var startTime = item.StartsAt.Hour + ":" + item.StartsAt.Minute + ":" + item.StartsAt.Second;
                var alertName = item.Labels.Alertname;
                var instance = item.Labels.Instance;
                var message = @$"خطا:"
                    + "\n"
                    + "تاریخ: " + startDate + "\n"
                    + "ساعت: " + startTime + "\n"
                    + "Title: " + alertName + "\n"
                    + item.Annotations.Summary;
                _logger.LogInformation(DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss"));
                _logger.LogInformation(message);
                //TODO : send alert as SMS, Whatsapp /* "https://api.callmebot.com/" */ or Telegram 
            }
            return Ok("Success");

        }

    }
}