using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface IIBetaApiService
    {
        Task<Ibeta1Response> Ibeta1Async(string image, object? contentBodyExternalRequest);
        Task<Ibeta2Response> Ibeta2Async(string imageOne, string imageTwo, object? contentBodyExternalRequest);
    }
}
