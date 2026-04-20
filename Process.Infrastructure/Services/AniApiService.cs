using Microsoft.Extensions.Configuration;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class AniApiService(
        IExternalApiClientService externalApiClientService,
        IConfiguration config,
        ITokenCacheService tokenCacheService
    ) : IAniApiService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;
        private readonly ITokenCacheService _tokenCacheService = tokenCacheService;

        public async Task<string> GetTokenAniAsync()
        {
            string key = $"Token:ANI:{nameof(Process)}";
            var tokenCache = await _tokenCacheService.GetTokenCacheAsync<string>(key);

            if (tokenCache != null)
            {
                return tokenCache;
            }

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlAni,
                ApiName = ApiName.LoginAuthAni,
                Body = new
                {
                    Username = _config.GetSection("AniService:Username").Value!,
                    Password = _config.GetSection("AniService:Password").Value!,
                    ApplicationCode = _config.GetSection("AniService:ApplicationCode").Value!
                }
            };

            string response = await _externalApiClientService.PostAsync<string>(externalApiClientRequest);

            await _tokenCacheService.SetTokenCacheAsync(key, response);

            return response;
        }

        public async Task<ValidationDocumentAniResponse> ValidationDocumentAniAsync(string documentNumber, string documentType)
        {
            string tokenAni = await GetTokenAniAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlAni,
                ApiName = ApiName.ValidationDocumentAni,
                Body = new
                {
                    CodigoAplicacion = _config.GetSection("AniService:ApplicationCode").Value!,
                    Documento = documentNumber,
                    TipoDocumento = documentType
                },
                Token = tokenAni,
                AuthorizationValue = _config.GetSection("AniService:AuthorizationValue").Value!
            };

            ValidationDocumentAniResponse response = await _externalApiClientService
                .PostAsync<ValidationDocumentAniResponse>(externalApiClientRequest);

            if (response.ResponseCode != "0")
            {
                throw new KeyNotFoundException(response.ResponseDescription);
            }

            return response!;
        }
    }
}
