using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nuget.LogService.Services;
using UITemplate.Application.Context;
using UITemplate.Domain.Interface;
using UITemplate.Domain.Repositories;
using UITemplate.Infrastructure.Repositories;

namespace UITemplate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoContext, MongoUnitWork>();
            services.AddScoped<IUITemplateRepository, UITemplateRepository>();

            var logAPIBaseURL = configuration.GetRequiredSection("ApiCentralLog:UrlBase").Value;
            var logAPIToken = configuration.GetRequiredSection("ApiCentralLog:Token").Value;

            services.AddScoped<ILogServices>(c =>
            {
                var logger = c.GetRequiredService<ILogger<LogServices>>();
                var clientFactory = c.GetRequiredService<IHttpClientFactory>();
                return new LogServices(logAPIBaseURL, logAPIToken, logger, clientFactory);
            });
            return services;
        }
    }
}
