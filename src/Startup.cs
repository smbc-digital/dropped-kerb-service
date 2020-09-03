using System.Diagnostics.CodeAnalysis;
using dropped_kerb_service.Utils.HealthChecks;
using dropped_kerb_service.Utils.ServiceCollectionExtensions;
using dropped_kerb_service.Utils.StorageProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockportGovUK.AspNetCore.Availability;
using StockportGovUK.AspNetCore.Availability.Middleware;
using StockportGovUK.AspNetCore.Middleware;
using StockportGovUK.NetStandard.Gateways.Extensions;
using StockportGovUK.NetStandard.Gateways.MailingService;
using StockportGovUK.NetStandard.Gateways.VerintService;

namespace dropped_kerb_service
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson();
            services.AddStorageProvider(Configuration);

            services.AddHttpClient<IVerintServiceGateway, VerintServiceGateway>(Configuration);
            services.AddHttpClient<IMailingServiceGateway, MailingServiceGateway>(Configuration);

            services.AddAvailability();

            services.RegisterServices()
                .RegisterIOptions(Configuration)
                .AddSwagger();

            services.AddHealthChecks()
                    .AddCheck<TestHealthCheck>("TestHealthCheck");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsEnvironment("local"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseMiddleware<Availability>();
            app.UseMiddleware<ApiExceptionHandling>();

            app.UseHealthChecks("/healthcheck", HealthCheckConfig.Options);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Dropped Kerb Service API");
            });
        }
    }
}
