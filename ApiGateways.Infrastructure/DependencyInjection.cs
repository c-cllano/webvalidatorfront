using ApiGateways.Domain.Services;
using ApiGateways.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nuget.LogService.Services;

namespace ApiGateways.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {


            var logAPIBaseURL = configuration.GetRequiredSection("ApiCentralLog:UrlBase").Value;
            var logAPIToken = configuration.GetRequiredSection("ApiCentralLog:Token").Value;

            services.AddScoped<ILogServices>(c =>
            {
                var logger = c.GetRequiredService<ILogger<LogServices>>();
                var clientFactory = c.GetRequiredService<IHttpClientFactory>();
                return new LogServices(logAPIBaseURL!, logAPIToken!, logger, clientFactory);
            });

            services.AddSingleton<ITokenCacheService, TokenCacheService>();

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = NormalizeBackslashes(configuration.GetConnectionString("OKeyConnection")!);
                options.SchemaName = "dbo";
                options.TableName = "TokenCache";
            });

            return services;
        }

        // Inicio código generado por GitHub Copilot
        private static string NormalizeBackslashes(string value)
        {
            while (value.Contains(@"\\"))
            {
                value = value.Replace(@"\\", @"\");
            }

            return value;
        }
        // Fin código generado por GitHub Copilot
    }
}
