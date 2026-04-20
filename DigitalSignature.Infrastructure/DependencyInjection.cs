using DigitalSignature.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nuget.LogService.Services;

namespace DigitalSignature.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<Domain.Repositories.IParameterClientRepository, Repositories.ParameterClientRepository>();

            services.AddTransient<Domain.Services.IExternalApiClientService, Services.ExternalApiClientService>();
            services.AddTransient<Domain.Services.ILoginEmissionService, Services.LoginEmissionService>();
            services.AddTransient<Domain.Services.ILoginSignatureService, Services.LoginSignatureService>();
            services.AddTransient<Domain.Services.ITemplateService, Services.TemplateService>();
            services.AddTransient<Domain.Services.IDocumentService, Services.DocumentService>();
            services.AddTransient<Domain.Services.ISignService, Services.SignService>();
            services.AddTransient<Domain.Services.ITokenCacheService, Services.TokenCacheService>();

            var logAPIBaseURL = configuration.GetRequiredSection("ApiCentralLog:UrlBase").Value;
            var logAPIToken = configuration.GetRequiredSection("ApiCentralLog:Token").Value;

            services.AddSingleton<SQLServerConnectionFactory>();

            // Inyectar LogServices con el ILogger correctamente
            services.AddScoped<ILogServices>(c =>
            {
                var logger = c.GetRequiredService<ILogger<LogServices>>();
                var clientFactory = c.GetRequiredService<IHttpClientFactory>();
                return new LogServices(logAPIBaseURL!, logAPIToken!, logger, clientFactory);
            });

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = NormalizeBackslashes(configuration.GetConnectionString("OKeyConnection")!);
                options.SchemaName = "dbo";
                options.TableName = "TokenCache";
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
