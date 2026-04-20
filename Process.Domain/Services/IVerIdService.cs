using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface IVerIdService
    {
        Task<VerIdResponse> VerIdAsync(string imageOne, string imageTwo, object? contentBodyExternalRequest);
    }
}
