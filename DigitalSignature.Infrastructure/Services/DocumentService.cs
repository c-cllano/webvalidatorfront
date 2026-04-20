using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using static DigitalSignature.Domain.Enums.Enumerations;

namespace DigitalSignature.Infrastructure.Services
{
    public class DocumentService(
        IExternalApiClientService externalApiClientService,
        ILoginEmissionService loginEmissionService
    ) : IDocumentService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly ILoginEmissionService _loginEmissionService = loginEmissionService;

        public async Task<ResultResponse<GenerateDocumentSignatureResponse>> GenerateDocumentAsync(
            long clientId,
            Guid templateSerial,
            IEnumerable<GenerateDocumentDataRequest> data
        )
        {
            ResultResponse<LoginEmissionResponse> responseToken = await _loginEmissionService
                .LoginEmissionAsync(clientId);

            var dataRequest = new List<object>();

            foreach (var item in data)
            {
                dataRequest.Add(new
                {
                    llave = item.Key,
                    valor = item.Value
                });
            }

            object contentBody = new
            {
                plantillaSerial = templateSerial,
                datos = dataRequest
            };

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlEmissionDocument,
                ApiName = ApiName.GenerateDocument,
                Body = contentBody,
                Token = responseToken.Data.Token
            };

            ResultResponse<GenerateDocumentSignatureResponse> response = await _externalApiClientService
                .PostAsync<ResultResponse<GenerateDocumentSignatureResponse>>(externalApiClientRequest);

            return response;
        }
    }
}
