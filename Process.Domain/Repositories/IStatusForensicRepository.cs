using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IStatusForensicRepository
    {
        Task<StatusForensic?> GetStatusByDescriptionAsync(string description);
    }
}
