using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonitoringExample.Api.Jobs;
using MonitoringExample.Api.Middlewares;
using MonitoringExample.Api.Monitoring;
using Prometheus;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<MetricConfig>(builder.Configuration.GetSection(MetricConfig.Key));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddScoped<MetricsRegistry>();
builder.Services.AddScoped<IMetricDataGenerator, MetricTestDataGenerator>();
builder.Services.AddHostedService<GenerateMonitoringValuesJob>();

builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options => options
    .WithTitle("Monitoring Example")
    .WithTheme(ScalarTheme.Moon)
    .WithDarkMode());

app.MapControllers();
app.MapMetrics();

app.UseMiddleware<RequestLoggerMiddleware>();

await app.RunAsync();