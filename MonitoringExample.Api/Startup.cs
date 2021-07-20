using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MonitoringExample.Api.Jobs;
using MonitoringExample.Api.Middlewares;
using MonitoringExample.Api.Monitoring;
using Newtonsoft.Json.Converters;

namespace MonitoringExample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.GetSection("MetricConfig").Bind(MetricConfig.Instance);

            services.AddControllers();
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MonitoringExample.Api", Version = "v1" });
            });
            var metrics = AppMetrics.CreateDefaultBuilder()
            .Build();
            services.AddMetrics();
            services.AddMetricsTrackingMiddleware();
            services.AddAppMetricsCollectors();
            services.AddAppMetricsGcEventsMetricsCollector();

            services.AddScoped<MetricsRegistry>();

            services.AddScoped<IMetricsManager, MetricsManager>();
            services.AddScoped<ICodeService, CodeService>();
            services.AddScoped<IMetricDataGenerator, MetricTestDataGenerator>();

            services.AddHostedService<GenerateMonitoringValuesJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MonitoringExample.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<RequestLoggerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
