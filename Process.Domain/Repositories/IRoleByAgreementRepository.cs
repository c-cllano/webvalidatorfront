using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IRoleByAgreementRepository
    {
        Task<long> AddAsync(RoleByAgreement roleByAgreement);
        Task<RoleByAgreement?> GetByIdAsync(long roleByAgreementId);
        Task<IEnumerable<RoleByAgreement>> GetByRoleIdAsync(long roleId);
        Task<IEnumerable<RoleByAgreement>> GetByAgreementIdAsync(int agreementId);
        Task<IEnumerable<RoleByAgreement>> GetAllAsync();
        Task UpdateAsync(RoleByAgreement roleByAgreement);
        Task DeleteAsync(long roleByAgreementId);
        Task<bool> ExistsAsync(long roleId, int agreementId);
        Task UpdateRoleAgreements(long roleId, IEnumerable<int> agreementIds, long updaterUserId);
    }
}
