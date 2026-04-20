using DigitalSignature.Domain.Entities;
using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Repositories;
using DigitalSignature.Domain.Services;
using DigitalSignature.Domain.Utilities;
using static DigitalSignature.Domain.Enums.Enumerations;

namespace DigitalSignature.Infrastructure.Services
{
    public class LoginSignatureService(
        IExternalApiClientService externalApiClientService,
        IParameterClientRepository parameterClientRepository,
        ITokenCacheService tokenCacheService
    ) : ILoginSignatureService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IParameterClientRepository _parameterClientRepository = parameterClientRepository;
        private readonly ITokenCacheService _tokenCacheService = tokenCacheService;

        public async Task<ResultResponse<LoginSignatureResponse>> LoginSignatureAsync(long clientId)
        {
            string key = $"Token:Signature:{nameof(DigitalSignature)}";
            var tokenCache = await _tokenCacheService.GetTokenCacheAsync<LoginSignatureResponse>(key);

            if (tokenCache != null)
            {
                return new ResultResponse<LoginSignatureResponse>
                {
                    Response = true,
                    Data = tokenCache
                };
            }

            object contentBody = await GetParametersClientAsync(clientId);

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlSignature,
                ApiName = ApiName.LoginSignature,
                Body = contentBody
            };

            ResultResponse<LoginSignatureResponse> response = await _externalApiClientService
                .PostAsync<ResultResponse<LoginSignatureResponse>>(externalApiClientRequest);

            if (response == null || !response.Response)
            {
                throw new KeyNotFoundException("Error obteniendo el token del proceso de Firmas Digitales");
            }

            await _tokenCacheService.SetTokenCacheAsync(key, response.Data);

            return response;
        }

        private async Task<object> GetParametersClientAsync(long clientId)
        {
            IEnumerable<ParameterClient> listParameterClient = await _parameterClientRepository
                .GetParameterClientByClientId(clientId)
                    ?? throw new KeyNotFoundException("No hay parámetros de cliente");

            var requiredParameters = new[]
            {
                Constants.User_Login_Signature,
                Constants.Password_Login_Signature,
                Constants.Code_Login_Signature
            };

            var missingParameters = requiredParameters
                .Where(rp => !listParameterClient.Any(p => p.ParameterName == rp))
                .ToList();

            if (missingParameters.Count != 0)
            {
                string messageError = "Faltan los siguientes parámetros: " + string.Join(", ", missingParameters);
                throw new KeyNotFoundException(messageError);
            }

            var user = listParameterClient
                .FirstOrDefault(p => p.ParameterName == Constants.User_Login_Signature)?.ParameterValue;

            var password = listParameterClient
                .FirstOrDefault(p => p.ParameterName == Constants.Password_Login_Signature)?.ParameterValue;

            var code = listParameterClient
                .FirstOrDefault(p => p.ParameterName == Constants.Code_Login_Signature)?.ParameterValue;

            object contentBody = new
            {
                usuario = user,
                contrasena = password,
                codigo = code
            };

            return contentBody;
        }
    }
}
