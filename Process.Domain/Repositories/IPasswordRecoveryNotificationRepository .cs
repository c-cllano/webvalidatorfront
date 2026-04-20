using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IPasswordRecoveryNotificationRepository
    {
        Task SaveAsync(PasswordRecoveryNotification passwordRecoveryNotification);
        Task SaveToken(PasswordRecoveryNotification passwordRecoveryNotification);
        Task<bool> GetByToken(string token);
    }
}
