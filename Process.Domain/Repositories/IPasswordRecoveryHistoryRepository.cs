using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IPasswordRecoveryHistoryRepository
    {
        Task SaveToken(PasswordRecoveryHistory passwordRecoveryHistory);
        Task<bool> GetByToken(string token, string email);
    }
}
