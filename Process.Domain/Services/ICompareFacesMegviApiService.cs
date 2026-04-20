using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface ICompareFacesMegviApiService
    {
        Task<FaceCompareMegviResponse> CompareFacesMegviApiAsync(string imageOne, string imageTwo, object? contentBodyExternalRequest);
    }
}
