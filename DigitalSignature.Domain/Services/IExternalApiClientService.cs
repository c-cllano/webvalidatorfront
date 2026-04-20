using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;

namespace DigitalSignature.Domain.Services
{
    public interface IExternalApiClientService
    {
        Task<T> GetAsync<T>(ExternalApiClientRequest externalApiClientRequest);
        Task<T> PostAsync<T>(ExternalApiClientRequest externalApiClientRequest);
        Task<T> PutAsync<T>(ExternalApiClientRequest externalApiClientRequest);
    }
}
