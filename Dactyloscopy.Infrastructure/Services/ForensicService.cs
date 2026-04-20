using Dactyloscopy.Domain.Parameters.ExternalApiClientParameters;
using Dactyloscopy.Domain.Services;
using Microsoft.Extensions.Configuration;
using static Dactyloscopy.Domain.Enums.Enumerations;

namespace Dactyloscopy.Infrastructure.Services
{
    public class ForensicService(
        IExternalApiClientService externalApiClientService,
        IConfiguration config
    ) : IForensicService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;

        public async Task<GetTokenResponse> GetTokenAsync()
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlDactyloscopy,
                ApiName = ApiName.GetToken,
                Body = new
                {
                    clientId = _config.GetSection("Forensic:ClientId").Value,
                    clientSecret = _config.GetSection("Forensic:ClientSecret").Value
                }
            };

            GetTokenResponse tokenResponse = await _externalApiClientService
                .PostAsync<GetTokenResponse>(externalApiClientRequest);

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse!.AccessToken))
            {
                throw new KeyNotFoundException("Error obteniendo el token del servicio de Dactiloscopia");
            }

            return tokenResponse;
        }

        public async Task<ForensicReviewResponse> RequestForensicReviewAsync(object contentBody)
        {
            GetTokenResponse tokenResponse = await GetTokenAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlDactyloscopy,
                ApiName = ApiName.RequestForensicReview,
                Body = contentBody,
                Token = tokenResponse.AccessToken
            };

            ForensicReviewResponse response = await _externalApiClientService
                .PostAsync<ForensicReviewResponse>(externalApiClientRequest);

            return response;
        }

        public async Task<ForensicStatusResponse> GetForensicStatusAsync(Guid txGuid)
        {
            GetTokenResponse tokenResponse = await GetTokenAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlDactyloscopy,
                ApiName = ApiName.GetForensicStatus,
                Token = tokenResponse.AccessToken,
                QueryParams = new Dictionary<string, string> { { "TxGuid", $"{txGuid}" } }
            };

            ForensicStatusResponse response = await _externalApiClientService
                .GetAsync<ForensicStatusResponse>(externalApiClientRequest);

            return response;
        }
    }
}
