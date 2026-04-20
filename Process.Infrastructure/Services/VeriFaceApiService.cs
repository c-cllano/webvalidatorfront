using Microsoft.Extensions.Configuration;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class VeriFaceApiService(
        IExternalApiClientService externalApiClientService,
        IConfiguration config
    ) : IVeriFaceApiService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;

        public async Task<VeriFaceResponse> GetTokenVeriFaceAsync()
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlVeriFace,
                ApiName = ApiName.GetTokenVeriface,
                CustomHeaders = new Dictionary<string, string>
                {
                    { "api-key", _config.GetSection("ApiKeyVeriFace")?.Value! },
                }
            };

            VeriFaceResponse response = await _externalApiClientService
                .GetAsync<VeriFaceResponse>(externalApiClientRequest);

            return response;
        }
    }
}
