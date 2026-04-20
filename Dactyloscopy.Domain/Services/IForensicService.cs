using Dactyloscopy.Domain.Parameters.ExternalApiClientParameters;

namespace Dactyloscopy.Domain.Services
{
    public interface IForensicService
    {
        Task<GetTokenResponse> GetTokenAsync();
        Task<ForensicReviewResponse> RequestForensicReviewAsync(object contentBody);
        Task<ForensicStatusResponse> GetForensicStatusAsync(Guid txGuid);
    }
}
