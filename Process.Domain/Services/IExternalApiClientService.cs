using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface IExternalApiClientService
    {
        Task<T> GetAsync<T>(ExternalApiClientRequest externalApiClientRequest);
        Task<T> PostAsync<T>(ExternalApiClientRequest externalApiClientRequest);
        Task<T> PutAsync<T>(ExternalApiClientRequest externalApiClientRequest);
        Task DeleteAsync<T>(ExternalApiClientRequest externalApiClientRequest);
    }
}
