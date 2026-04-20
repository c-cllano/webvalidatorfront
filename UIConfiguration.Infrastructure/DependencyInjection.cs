using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nuget.LogService.Services;
using UIConfiguration.Application.Context;
using UIConfiguration.Domain.Interface;
using UIConfiguration.Domain.Repositories;
using UIConfiguration.Domain.Services;
using UIConfiguration.Infrastructure.Repositories;

namespace UIConfiguration.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthService, Services.AuthService>();
            services.AddSingleton<IMongoContext, MongoUnitWork>();
            services.AddScoped<IUIConfigurationRepository, UIConfigurationRepositoriy>();

            var logAPIBaseURL = configuration.GetRequiredSection("ApiCentralLog:UrlBase").Value;
            var logAPIToken = configuration.GetRequiredSection("ApiCentralLog:Token").Value;

            services.AddScoped<ILogServices>(c =>
            {
                var logger = c.GetRequiredService<ILogger<LogServices>>();
                var clientFactory = c.GetRequiredService<IHttpClientFactory>();
                return new LogServices(logAPIBaseURL!, logAPIToken!, logger, clientFactory);
            });
            return services;
        }
    }
}
