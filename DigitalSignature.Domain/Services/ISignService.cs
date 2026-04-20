using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;

namespace DigitalSignature.Domain.Services
{
    public interface ISignService
    {
        Task<ResultResponse<SignSignatureResponse>> SignAsync(long clientId, string base64, bool tsa, string nombreFirma, bool logoDefault, string base64LogoPersonalizado);
    }
}
