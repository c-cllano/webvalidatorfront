using Dactyloscopy.Domain.Entities;

namespace Dactyloscopy.Domain.Repositories
{
    public interface IStatusForensicRepository
    {
        Task<StatusForensic?> GetStatusByIdAsync(long statusForensicId);
        Task<StatusForensic?> GetStatusByDescriptionAsync(string description);
    }
}
