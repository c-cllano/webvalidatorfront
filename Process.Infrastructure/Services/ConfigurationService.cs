using Microsoft.Extensions.Configuration;
using Process.Domain.Services;

namespace Process.Infrastructure.Services
{
    public class ConfigurationService(IConfiguration configuration) : IConfigurationService
    {
        private readonly IConfiguration _configuration = configuration;


        public string GetConfiguration(string key)
        {
            var value = _configuration.GetSection(key)?.Value;
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException($"La clave {key} no se encuentra en la configuración.");
            }
            return value;
        }
    }
}
