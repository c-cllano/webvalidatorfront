using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using static DigitalSignature.Domain.Enums.Enumerations;

namespace DigitalSignature.Infrastructure.Services
{
    public class TemplateService(
        IExternalApiClientService externalApiClientService,
        ILoginEmissionService loginEmissionService
    ) : ITemplateService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly ILoginEmissionService _loginEmissionService = loginEmissionService;

        public async Task<ResultResponse<GetTemplateSignatureResponse>> GetTemplateAsync(
            long clientId,
            Guid templateSerial
        )
        {
            ResultResponse<LoginEmissionResponse> responseToken = await _loginEmissionService
                .LoginEmissionAsync(clientId);

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlEmissionDocument,
                ApiName = ApiName.GetTemplate,
                Token = responseToken.Data.Token,
                QueryParams = new Dictionary<string, string> { { "plantillaSerial", $"{templateSerial}" } }
            };

            ResultResponse<GetTemplateSignatureResponse> response = await _externalApiClientService
                .GetAsync<ResultResponse<GetTemplateSignatureResponse>>(externalApiClientRequest);

            return response;
        }

        public async Task<ResultResponse<GetTemplateFieldsSignatureResponse>> GetTemplateFieldsAsync(long clientId, Guid templateSerial)
        {
            ResultResponse<LoginEmissionResponse> responseToken = await _loginEmissionService
                .LoginEmissionAsync(clientId);

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlEmissionDocument,
                ApiName = ApiName.GetTemplateFields,
                Token = responseToken.Data.Token,
                QueryParams = new Dictionary<string, string> { { "PlantillaSerial", $"{templateSerial}" } }
            };

            ResultResponse<GetTemplateFieldsSignatureResponse> response = await _externalApiClientService
                .GetAsync<ResultResponse<GetTemplateFieldsSignatureResponse>>(externalApiClientRequest);

            return response;
        }

        public async Task<ResultResponse<CreateTemplateSignatureResponse>> CreateTemplateAsync(
            long clientId,
            string templateBase64,
            string templateName
        )
        {
            ResultResponse<LoginEmissionResponse> responseToken = await _loginEmissionService
                .LoginEmissionAsync(clientId);

            object contentBody = new
            {
                plantillaBase64 = templateBase64,
                plantillaNombre = templateName
            };

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlEmissionDocument,
                ApiName = ApiName.CreateTemplate,
                Body = contentBody,
                Token = responseToken.Data.Token
            };

            ResultResponse<CreateTemplateSignatureResponse> response = await _externalApiClientService
                .PostAsync<ResultResponse<CreateTemplateSignatureResponse>>(externalApiClientRequest);

            return response;
        }

        public async Task<ResultResponse<UpdateTemplateSignatureResponse>> UpdateTemplateAsync(
            long clientId,
            Guid templateSerial,
            string templateBase64,
            string templateName
        )
        {
            ResultResponse<LoginEmissionResponse> responseToken = await _loginEmissionService
                .LoginEmissionAsync(clientId);

            object contentBody = new
            {
                plantillaBase64 = templateBase64,
                plantillaNombre = templateName,
                plantillaSerial = templateSerial
            };

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlEmissionDocument,
                ApiName = ApiName.UpdateTemplate,
                Body = contentBody,
                Token = responseToken.Data.Token
            };

            ResultResponse<UpdateTemplateSignatureResponse> response = await _externalApiClientService
                .PutAsync<ResultResponse<UpdateTemplateSignatureResponse>>(externalApiClientRequest);

            return response;
        }
    }
}
