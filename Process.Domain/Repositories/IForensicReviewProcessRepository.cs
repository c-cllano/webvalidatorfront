using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IForensicReviewProcessRepository
    {
        Task<ForensicReviewProcess?> GetForensicReviewProcessByValidationProcessIdAsync(long validationProcessId);
        Task<ForensicReviewProcess> SaveForensicReviewProcessAsync(ForensicReviewProcess forensicReviewProcess);
        Task<ForensicReviewProcess> UpdateForensicReviewProcessAsync(ForensicReviewProcess forensicReviewProcess);
    }
}
