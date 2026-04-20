using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IReasonRepository
    {
        Task<Reason?> GetReasonById(long id);
    }
}
