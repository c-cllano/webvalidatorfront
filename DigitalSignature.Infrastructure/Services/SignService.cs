using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using static DigitalSignature.Domain.Enums.Enumerations;

namespace DigitalSignature.Infrastructure.Services
{
    public class SignService(
        IExternalApiClientService externalApiClientService,
        ILoginSignatureService loginSignatureService
    ) : ISignService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly ILoginSignatureService _loginSignatureService = loginSignatureService;

        public async Task<ResultResponse<SignSignatureResponse>> SignAsync(long clientId, string base64, bool tsa,string nombreFirma, bool logoDefault,string base64LogoPersonalizado)
        {
            ResultResponse<LoginSignatureResponse> responseToken = await _loginSignatureService
                .LoginSignatureAsync(clientId);

            object contentBody = new
            {
                base64,
                tsa,
                nombreFirma,
                logoDefault,
                base64LogoPersonalizado
            };

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlSignature,
                ApiName = ApiName.Sign,
                Body = contentBody,
                Token = responseToken.Data.Token
            };

            ResultResponse<SignSignatureResponse> response = await _externalApiClientService
                .PostAsync<ResultResponse<SignSignatureResponse>>(externalApiClientRequest);

            return response;
        }
    }
}
