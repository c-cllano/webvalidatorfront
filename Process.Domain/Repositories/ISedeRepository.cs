using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface ISedeRepository
    {
        Task<Sede?> GetSedeById(long sedeId);
    }
}
