using MonitoringExample.Api.Enums;
using System;
using System.Collections.Generic;
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
            var temp1 = RandomGenerator(Constants.MinNum, Constants.MidNum);
            var temp2 = RandomGenerator(Constants.MinNum, Constants.MidNum);

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
            var randPercent = RandomGenerator(Constants.MinNum, Constants.MaxNum);
            var temp1 = RandomGenerator(Constants.MinNum, Constants.MidNum);
            var temp2 = RandomGenerator(Constants.MinNum, Constants.MidNum);

            if (temp1 + temp2 > 70)
                errorPercent *= 1.3f;
            else if (temp1 + temp2 > 50)
                errorPercent *= 0.7f;

            var statusCode = HttpStatusCode.OK; // In most cases
            if (randPercent < errorPercent || (randPercent + 15 < errorPercent && service == ExternalServices.Shopping2 && requestCount % 3 == 0)) // Shopping2 errors greather than other services
            {
                statusCode = HttpStatusCodes[RandomGenerator(1, 11)];
            }

            return statusCode;
        }

        internal static int GenerateRandomResponseTime()
        {
            var averageResponseTime = MetricConfig.Instance.AverageResponseTime;
            // Generate two random number and distribute responseTime based this two numbers
            var temp1 = RandomGenerator(Constants.MinNum, Constants.MidNum);
            var temp2 = RandomGenerator(Constants.MinNum, Constants.MidNum);

            var keys = new List<int>(Constants.ResponseTimeDistribution.Keys);
            var nearestValue = keys.BinarySearch(temp1 + temp2);
            return Convert.ToInt32(Constants.ResponseTimeDistribution[keys[nearestValue > 0? nearestValue: (nearestValue * -1) - 1]] * averageResponseTime);
        }
             
    }
}
