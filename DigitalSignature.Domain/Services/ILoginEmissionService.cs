using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;

namespace DigitalSignature.Domain.Services
{
    public interface ILoginEmissionService
    {
        Task<ResultResponse<LoginEmissionResponse>> LoginEmissionAsync(long clientId);
    }
}
