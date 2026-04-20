using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IAgreementByUserRepository
    {
        Task<long> AddAsync(AgreementByUser agreementByUser);
        Task<AgreementByUser?> GetByUserIdAgreementAsync(int userId, int agreementId);
        Task<IEnumerable<AgreementByUser>> GetByUserIdAsync(int userId);
        Task UpdateUserAgreements(int userId, IEnumerable<int> agreementIds, long updaterUserId);
    }
}
