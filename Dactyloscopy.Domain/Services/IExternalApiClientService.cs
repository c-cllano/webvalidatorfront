using Dactyloscopy.Domain.Parameters.ExternalApiClientParameters;

namespace Dactyloscopy.Domain.Services
{
    public interface IExternalApiClientService
    {
        Task<T> GetAsync<T>(ExternalApiClientRequest externalApiClientRequest);
        Task<T> PostAsync<T>(ExternalApiClientRequest externalApiClientRequest);
    }
}
