using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;

namespace DigitalSignature.Domain.Services
{
    public interface ILoginSignatureService
    {
        Task<ResultResponse<LoginSignatureResponse>> LoginSignatureAsync(long clientId);
    }
}
