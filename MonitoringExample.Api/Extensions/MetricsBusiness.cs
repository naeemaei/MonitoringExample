using MonitoringExample.Api.Enums;
using System;
using System.Net;

namespace MonitoringExample.Api.Extensions
{
    public static partial class Extension
    {
        public readonly static HttpStatusCode[] HttpStatusCodes = new HttpStatusCode[]
        {
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.Forbidden,
            HttpStatusCode.MethodNotAllowed,
            HttpStatusCode.NotFound,
            HttpStatusCode.RequestUriTooLong,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.TooManyRequests
        };
        internal static ExternalServices GenerateRandomExternalService(int requestCount)
        {
            var service = ExternalServices.Internal;

            if (requestCount % 25 == 0) // 1 of each 25 requests from external services
            {
                service = (ExternalServices)RandomGenerator(1, 3);
            }

            return service;

        }

        internal static Endpoints GenerateRandomEndpoint()
        {
            var temp1 = RandomGenerator(0, 50);
            var temp2 = RandomGenerator(0, 50);

            var endpoint = (temp1 + temp2) switch
            {
                < 45 => Endpoints.Search,
                < 70 => Endpoints.ProductDetailPage,
                < 80 => Endpoints.Ordering,
                < 85 => Endpoints.Payment,
                _ => (Endpoints)RandomGenerator(1, 4)
            };

            return endpoint;

        }

        internal static HttpStatusCode GenerateRandomStatusCode(int requestCount, ExternalServices service)
        {
            var errorPercent = MetricConfig.Instance.ErrorPercent;
            var statusCode = HttpStatusCode.OK; // In most cases
            if (RandomGenerator(0, 100) < errorPercent)
            {
                statusCode = (HttpStatusCode)HttpStatusCodes[RandomGenerator(1, 11)];
                if (service == ExternalServices.Shopping2) // Shopping2 errors greather than other services
                {
                    if (requestCount % 3 == 0)
                    {
                        statusCode = (HttpStatusCode)HttpStatusCodes[RandomGenerator(1, 11)];
                    }
                }
            }

            return statusCode;
        }

        internal static int GenerateRandomResponseTime()
        {
            var averageResponseTime = MetricConfig.Instance.AverageResponseTime;
            // Generate two random number and distribute responseTime based this two numbers
            var temp1 = RandomGenerator(0, 50);
            var temp2 = RandomGenerator(0, 50);

            int responseTime;

            if (temp1 + temp2 >= 95) // 5% of requests response time between 3 and 6 second
                responseTime = (int)RandomGenerator(averageResponseTime * 3, averageResponseTime * 6);

            else if (temp1 + temp2 >= 85)
                responseTime = (int)RandomGenerator(averageResponseTime * 2, averageResponseTime * 3); // 10% of requests response time between 2 and 3 second

            else if (temp1 + temp2 >= 65)
                responseTime = (int)RandomGenerator(Convert.ToInt32(averageResponseTime * 1.5), averageResponseTime * 2); // 20% of requests response time between 1.5 and 2 second

            else if (temp1 + temp2 >= 50)
                responseTime = (int)RandomGenerator(averageResponseTime, Convert.ToInt32(averageResponseTime * 1.5)); // 15% of requests response time between 1 and 1.5 second

            else if (temp1 + temp2 >= 35)
                responseTime = (int)RandomGenerator(Convert.ToInt32(averageResponseTime * 0.75), averageResponseTime); // 15% of requests response time between 0.75 and 1 second

            else
                responseTime = (int)RandomGenerator(Convert.ToInt32(averageResponseTime * 0.1), Convert.ToInt32(averageResponseTime * 0.75)); // 35% of requests response time between 0.1 and 0.75 second

            return responseTime;
        }
    }
}
