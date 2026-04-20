using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Process.Domain.Exceptions;
using Process.Domain.Parameters.Context;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using Process.Infrastructure.Utility;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class ReconoserApiService(
        IExternalApiClientService externalApiClientService,
        IConfiguration config,
        ITokenCacheService tokenCacheService,
        ReconoserContext context
    ) : IReconoserApiService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;
        private readonly ITokenCacheService _tokenCacheService = tokenCacheService;
        private readonly ReconoserContext _context = context;

        public async Task<TokenReconoserIDResponse> GetTokenReconoserAsync()
        {
            string key = $"Token:ReconoserID:{nameof(Process)}{_context.AgreementGUID}";
            var tokenCache = await _tokenCacheService.GetTokenCacheAsync<TokenReconoserIDResponse>(key);

            if (tokenCache != null)
            {
                return new TokenReconoserIDResponse
                {
                    AccessToken = tokenCache.AccessToken,
                    RefreshToken = tokenCache.RefreshToken,
                    ErrorDescription = tokenCache.ErrorDescription,
                    ExpiresIn = tokenCache.ExpiresIn
                };
            }

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser1,
                ApiName = ApiName.TokenReconoser,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser1 : null,
                Body = new
                {
                    ClientId = _config.GetSection("ApiClient").Value,
                    ClientSecret = _config.GetSection("ApiKey").Value
                }
            };

            TokenReconoserIDResponse tokenExternalResponse = await _externalApiClientService
                .PostAsync<TokenReconoserIDResponse>(externalApiClientRequest);

            if (tokenExternalResponse == null || string.IsNullOrEmpty(tokenExternalResponse!.AccessToken) || !string.IsNullOrEmpty(tokenExternalResponse!.ErrorDescription))
            {
                throw new KeyNotFoundException("Error obteniendo el token de Reconoser");
            }

            await _tokenCacheService.SetTokenCacheAsync(key, tokenExternalResponse, tokenExternalResponse.ExpiresIn);

            return tokenExternalResponse;
        }

        public async Task<SaveDocumentBothSidesReconoserIDResponse> SaveDocumentBothSidesReconoserAsync(object contentBody, object? contentBodyExternalRequest)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser2,
                ApiName = ApiName.SaveDocumentBothSides,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser2 : null,
                Body = contentBody,
                BodyExternalRequest = contentBodyExternalRequest,
                Token = tokenExternalResponse.AccessToken
            };

            SaveDocumentBothSidesReconoserIDResponse response = await _externalApiClientService
                .PostAsync<SaveDocumentBothSidesReconoserIDResponse>(externalApiClientRequest);

            var error = response.RespuestaTransaccion?.ErrorEntransaccion?.FirstOrDefault();

            ValidateCodeAndDescription(error);

            return response;
        }

        public async Task<CompareFacesReconoserIDResponse> CompareFacesReconoserAsync(object contentBody, object? contentBodyExternalRequest)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser1,
                ApiName = ApiName.CompareFaces,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser1 : null,
                Body = contentBody,
                BodyExternalRequest = contentBodyExternalRequest,
                Token = tokenExternalResponse.AccessToken
            };

            CompareFacesReconoserIDResponse response = await _externalApiClientService
                .PostAsync<CompareFacesReconoserIDResponse>(externalApiClientRequest);

            return response;
        }

        public async Task<SaveBiometricReconoserIDResponse> SaveBiometricReconoserAsync(object contentBody, object? contentBodyExternalRequest)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser2,
                ApiName = ApiName.SaveBiometric,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser2 : null,
                Body = contentBody,
                BodyExternalRequest = contentBodyExternalRequest,
                Token = tokenExternalResponse.AccessToken
            };

            SaveBiometricReconoserIDResponse response = await _externalApiClientService
                .PostAsync<SaveBiometricReconoserIDResponse>(externalApiClientRequest);

            var error = response.RespuestaTransaccion?.ErrorEntransaccion?.FirstOrDefault();

            ValidateCodeAndDescription(error);

            return response;
        }

        // Inicio código generado por GitHub Copilot
        // Método generado por GitHub Copilot
        public async Task<SaveBiometricReconoserIDResponse> SaveBiometricV3ReconoserAsync(
            object contentMultipartForm,
            object? contentBodyExternalRequest,
            IFormFile biometric,
            IFormFile biometricGesture
        )
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser2,
                ApiName = ApiName.SaveBiometricV3,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser2 : null,
                Body = contentMultipartForm,
                BodyExternalRequest = contentBodyExternalRequest,
                Token = tokenExternalResponse.AccessToken,
                Files = new Dictionary<string, byte[]>
                {
                    { "valor", await biometric.ToBytesAsync() },
                    { "biometriaGesto", await biometricGesture.ToBytesAsync() }
                }
            };

            SaveBiometricReconoserIDResponse response = await _externalApiClientService
                .PostAsync<SaveBiometricReconoserIDResponse>(externalApiClientRequest);

            var error = response.RespuestaTransaccion?.ErrorEntransaccion?.FirstOrDefault();

            ValidateCodeAndDescription(error);

            return response;
        }
        // Fin código generado por GitHub Copilot

        public async Task<StatusValidationRequestReconoserIDResponse> StatusValidationRequestReconoserAsync(object contentBody)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser1,
                ApiName = ApiName.StatusValidationRequest,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser1 : null,
                Body = contentBody,
                Token = tokenExternalResponse.AccessToken
            };

            StatusValidationRequestReconoserIDResponse response = await _externalApiClientService
                .PostAsync<StatusValidationRequestReconoserIDResponse>(externalApiClientRequest);

            return response;
        }

        public async Task<ValidateBiometricReconoserIDResponse> ValidateBiometricReconoserAsync(object contentBody, object? contentBodyExternalRequest)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser2,
                ApiName = ApiName.ValidateBiometric,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser2 : null,
                Body = contentBody,
                BodyExternalRequest = contentBodyExternalRequest,
                Token = tokenExternalResponse.AccessToken
            };

            ValidateBiometricReconoserIDResponse response = await _externalApiClientService
                .PostAsync<ValidateBiometricReconoserIDResponse>(externalApiClientRequest);

            var error = response.RespuestaTransaccion?.ErrorEntransaccion?.FirstOrDefault();

            ValidateCodeAndDescription(error);

            return response;
        }

        // Inicio código generado por GitHub Copilot
        // Método generado por GitHub Copilot
        public async Task<ValidateBiometricReconoserIDResponse> ValidateBiometricV3ReconoserAsync(
            object contentMultipartForm,
            object? contentBodyExternalRequest,
            IFormFile biometric,
            IFormFile biometricGesture
        )
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser2,
                ApiName = ApiName.ValidateBiometricV3,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser2 : null,
                Body = contentMultipartForm,
                BodyExternalRequest = contentBodyExternalRequest,
                Token = tokenExternalResponse.AccessToken,
                Files = new Dictionary<string, byte[]>
                {
                    { "biometria", await biometric.ToBytesAsync() },
                    { "biometriaGesto", await biometricGesture.ToBytesAsync() }
                }
            };

            ValidateBiometricReconoserIDResponse response = await _externalApiClientService
                .PostAsync<ValidateBiometricReconoserIDResponse>(externalApiClientRequest);

            var error = response.RespuestaTransaccion?.ErrorEntransaccion?.FirstOrDefault();

            ValidateCodeAndDescription(error);

            return response;
        }
        // Fin código generado por GitHub Copilot

        public async Task<ConsultValidationReconoserIDResponse> ConsultValidationReconoserAsync(object contentBody)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser1,
                ApiName = ApiName.ConsultValidation,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser1 : null,
                Body = contentBody,
                Token = tokenExternalResponse.AccessToken
            };

            ConsultValidationReconoserIDResponse response = await _externalApiClientService
                .PostAsync<ConsultValidationReconoserIDResponse>(externalApiClientRequest);

            return response;
        }

        // Inicio código generado por GitHub Copilot
        public async Task<TransactionReconoserIDResponse> CancelProcessReconoserAsync(object contentBody)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser2,
                ApiName = ApiName.CancelProcess,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser2 : null,
                Body = contentBody,
                BodyExternalRequest = contentBody,
                Token = tokenExternalResponse.AccessToken
            };

            TransactionReconoserIDResponse response = await _externalApiClientService
                .PostAsync<TransactionReconoserIDResponse>(externalApiClientRequest);

            return response;
        }
        // Fin código generado por GitHub Copilot

        public async Task<string> ConsultValidationReconoserLegacyAsync(object contentBody)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser1,
                ApiName = ApiName.ConsultValidation,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser1 : null,
                Body = contentBody,
                Token = tokenExternalResponse.AccessToken
            };

            string response = await _externalApiClientService
                .PostAsync<string>(externalApiClientRequest);

            return response;
        }

        public async Task<ConsultAgreementProcessReconoserIDResponse> ConsultAgreementProcessReconoserAsync(Guid agreementProcessGuid)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser2,
                ApiName = ApiName.ConsultAgreementProcess,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser2 : null,
                Token = tokenExternalResponse.AccessToken,
                QueryParams = new Dictionary<string, string> { { "procesoConvenioGuid", $"{agreementProcessGuid}" } }
            };

            ConsultAgreementProcessReconoserIDResponse response = await _externalApiClientService
                .GetAsync<ConsultAgreementProcessReconoserIDResponse>(externalApiClientRequest);

            return response;
        }

        public async Task<TempKeysReconoserIDResponse> GetTempKeysAsync(Guid validationProcessGuid)
        {
            TokenReconoserIDResponse tokenExternalResponse = await GetTokenReconoserAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = _context.ChangeUrl == true ? null : ApiName.BaseUrlReconoser2,
                ApiName = ApiName.GetTempKeys,
                BaseUrlReconoser = _context.ChangeUrl == true ? _context.BaseUrlReconoser2 : null,
                Token = tokenExternalResponse.AccessToken,
                QueryParams = new Dictionary<string, string> { { "procesoConvenioGuid", $"{validationProcessGuid}" } }
            };

            TempKeysReconoserIDResponse response = await _externalApiClientService
                .GetAsync<TempKeysReconoserIDResponse>(externalApiClientRequest);

            return response;
        }

        // Inicio código generado por GitHub Copilot
        // Validar código general para las peticiones
        private static void ValidateCodeAndDescription(
            ErrorInTransactionReconoserIDResponse? errorResponse
        )
        {
            string code = errorResponse?.Codigo ?? string.Empty;
            string description = errorResponse?.Descripcion ?? string.Empty;

            if (
                !string.IsNullOrEmpty(code) &&
                !string.IsNullOrEmpty(description) &&
                code.Equals("0") &&
                description.Contains("The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.")
            )
            {
                throw new GenericException("No fue posible procesar la imagen enviada. Por favor, intenta nuevamente o contacta a soporte si el problema persiste.");
            }
        }
        // Fin código generado por GitHub Copilot
    }
}
