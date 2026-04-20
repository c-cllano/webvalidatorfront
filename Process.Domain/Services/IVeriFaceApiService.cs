using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface IVeriFaceApiService
    {
        Task<VeriFaceResponse> GetTokenVeriFaceAsync();
    }
}
