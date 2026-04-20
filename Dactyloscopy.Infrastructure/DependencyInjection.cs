using Dactyloscopy.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nuget.LogService.Services;

namespace Dactyloscopy.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));

            services.AddScoped<Domain.Repositories.IStatusForensicRepository, Repositories.StatusForensicRepository>();
            services.AddScoped<Domain.Repositories.IForensicReviewProcessRepository, Repositories.ForensicReviewProcessRepository>();
            services.AddTransient<Domain.Services.IForensicService, Services.ForensicService>();
            services.AddTransient<Domain.Services.IExternalApiClientService, Services.ExternalApiClientService>();
            services.AddSingleton<SQLServerConnectionFactory>();

            var logAPIBaseURL = configuration.GetRequiredSection("ApiCentralLog:UrlBase").Value;
            var logAPIToken = configuration.GetRequiredSection("ApiCentralLog:Token").Value;

            // Inyectar LogServices con el ILogger correctamente
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
