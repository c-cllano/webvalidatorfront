using Microsoft.Extensions.Configuration;
using Process.Domain.Exceptions;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class WhatsappService(
        IExternalApiClientService externalApiClientService,
        IConfiguration config
    ) : IWhatsappService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;

        public async Task<LoginWhatsappResponse> GetTokenWhatsappAsync()
        {
            object contentBody = new
            {
                codigo = _config.GetSection("WhatsappService:CodeWhatsapp")?.Value!,
                user = _config.GetSection("WhatsappService:UserWhatsapp")?.Value!,
                password = _config.GetSection("WhatsappService:PasswordWhatsapp")?.Value!
            };

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlWhatsapp,
                ApiName = ApiName.LoginWhatsapp,
                Body = contentBody
            };

            LoginWhatsappResponse response = await _externalApiClientService
                .PostAsync<LoginWhatsappResponse>(externalApiClientRequest);

            return response;
        }

        public async Task<RequestWhatsappResponse> SendRequestWhatsappAsync(object contentBody)
        {
            LoginWhatsappResponse token = await GetTokenWhatsappAsync();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlWhatsapp,
                ApiName = ApiName.CreateRequestWhatsapp,
                Body = contentBody,
                Token = token.Token
            };

            RequestWhatsappResponse response = await _externalApiClientService
                .PostAsync<RequestWhatsappResponse>(externalApiClientRequest);

            if (response is null || !response.SessionInitialized)
            {
                throw new BusinessException($"Error al enviar la solicitud a Whatsapp: {response?.Message}", 500);
            }

            return response!;
        }
    }
}
