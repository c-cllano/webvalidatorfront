using Dactyloscopy.Domain.Entities;

namespace Dactyloscopy.Domain.Repositories
{
    public interface IForensicReviewProcessRepository
    {
        Task<ForensicReviewProcess?> GetForensicReviewProcessByIdAsync(long forensicReviewProcessId);
        Task<IEnumerable<ForensicReviewProcess>> GetForensicReviewProcessInReviewAsync(long statusForensicId);
        Task<ForensicReviewProcess> SaveForensicReviewProcessAsync(ForensicReviewProcess forensicReviewProcess);
        Task<ForensicReviewProcess> UpdateForensicReviewProcessAsync(ForensicReviewProcess forensicReviewProcess);
    }
}
