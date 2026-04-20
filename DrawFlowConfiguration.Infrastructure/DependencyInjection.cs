using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nuget.LogService.Services;
using DrawFlowConfiguration.Application.Context;
using DrawFlowConfiguration.Domain.Interfaces;
using DrawFlowConfiguration.Infrastructure.Data;
using DrawFlowConfiguration.Infrastructure.Repositories;
using DrawFlowConfiguration.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrawFlowConfiguration.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = NormalizeBackslashes(configuration.GetConnectionString("OKeyWorkflowConnection")!) ?? throw new ArgumentNullException(nameof(configuration));

            services.AddScoped<Domain.Repositories.IValidationTransaction, Repositories.ValidationTransactionRepository>();

            services.AddSingleton<IMongoContext, MongoUnitWork>();
            services.AddScoped<IJsonWorkflowRepository, JsonWorkflowRepository>();
            services.AddScoped<IScreenFlowRepository, ScreenFlowRepository>();
            services.AddScoped<Domain.Repositories.ICountriesAndRegionsRepository, Repositories.CountriesAndRegionsRepository>();
            services.AddScoped<Domain.Repositories.IDocumentTypeByCountryRepository, Repositories.DocumentTypeByCountryRepository>();
            services.AddDbContext<ContextSqlServerDB>(options =>options.UseSqlServer(connectionString));
            services.AddSingleton(new SQLServerConnectionFactory(connectionString));
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

        private static string NormalizeBackslashes(string value)
        {
            while (value.Contains(@"\\"))
            {
                value = value.Replace(@"\\", @"\");
            }

            return value;
        }

    }

}
