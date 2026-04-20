using Microsoft.Extensions.Configuration;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Parameters.Recapcha;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class RecaptchaServices(IExternalApiClientService externalApiClientService, IConfiguration config) : IRecaptchaServices
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;

        public async Task<RecaptchaResponse> GetValidateRecaptchaAsync(string Response)
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlRecaptcha,
                ApiName = ApiName.GetValidateRecaptcha,
                QueryParams = new Dictionary<string, string> { { "secret", $"{_config.GetSection("RecaptchaSecret")?.Value!}" },{ "response", $"{Response}" } }
            };

            RecaptchaResponse response = await _externalApiClientService
                .GetAsync<RecaptchaResponse>(externalApiClientRequest);

            return response;
        }
    }
}
