using DrawFlowProcess.Application.Context;
using DrawFlowProcess.Domain.Interface;
using DrawFlowProcess.Domain.Repositories;
using DrawFlowProcess.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nuget.LogService.Services;

namespace DrawFlowProcess.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoContext, MongoUnitWork>();
            services.AddScoped<IDrwaFlowProcessRepository, DrawFlowProcessRepository>();
            services.AddScoped<IClientInfoRepository, ClientInfoRepository>();
            services.AddScoped<IGlobalConfigurationRepository, GlobalConfigurationRepository>();

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
