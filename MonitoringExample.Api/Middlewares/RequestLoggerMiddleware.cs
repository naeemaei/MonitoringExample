﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MonitoringExample.Api.Enums;
using MonitoringExample.Api.Monitoring;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringExample.Api.Middlewares
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggerMiddleware> _logger;
        private MetricsRegistry _metricsRegistry;
        private Stopwatch timer = new();

        public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, MetricsRegistry metricsRegistry)
        {
            _metricsRegistry = metricsRegistry;
            timer.Start();
            var status = configuration.GetValue<bool>("RegisterMetricsStatus");
            if (Ignore(context) || !status)
                await _next.Invoke(context);
            else
            {
                string response = string.Empty;
                try
                {
                    var request = await FormatRequest(context.Request);
                    var originalBodyStream = context.Response.Body;

                    using var bodyStream = new MemoryStream();
                    context.Response.Body = bodyStream;

                    await _next.Invoke(context);

                    response = await FormatResponse(bodyStream);
                    await bodyStream.CopyToAsync(originalBodyStream);

                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex?.StackTrace}");
                }
                finally
                {
                    timer.Stop();
                    ExternalServices service = ExternalServices.Internal;
                    if (context.Request.Query.TryGetValue("getFromExternalService", out var value))
                    {
                        if (Convert.ToBoolean(value))
                        {
                            service = (ExternalServices)Extensions.Extension.RandomGenerator(1, 3);
                        }
                    }

                    Client app = Client.Website;
                    if (context.Request.Query.TryGetValue("application", out value))
                    {
                        app = (Client)Convert.ToInt32(value);
                    }

                    Dictionary<string, object> labels = new()
                    {
                        { "application", app.ToString() },
                        { "service", service },
                    };

                    await _metricsRegistry.RegisterMetrics(context.Response.StatusCode, timer.ElapsedMilliseconds, labels);
                }
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var bodyAsText = string.Empty;
            try
            {
                request.EnableBuffering();

                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                bodyAsText = Encoding.UTF8.GetString(buffer);

            }
            finally
            {
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            return $"{request.Method} {request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private static async Task<string> FormatResponse(Stream bodyStream)
        {
            var plainBodyText = string.Empty;
            try
            {
                bodyStream.Seek(0, SeekOrigin.Begin);
                plainBodyText = await new StreamReader(bodyStream).ReadToEndAsync();
            }
            finally
            {
                bodyStream.Seek(0, SeekOrigin.Begin);
            }

            return plainBodyText;
        }

        private bool Ignore(HttpContext context)
            => !context.Request.Path.Value.StartsWith("/api");
    }
}
