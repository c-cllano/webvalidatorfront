using Process.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Domain.Repositories
{
    public interface IRoleAgreementByUserRepository
    {
        Task<IEnumerable<RoleAgreementByUser?>> GetByUserRoleAndAgreemenmt(int userId, long roleId, int agreementId);
        Task<long> AddAsync(RoleAgreementByUser roleByUser);
        Task<IEnumerable<UserRoleAgreementNewDto>> GetUserRolesAndAgreementsAsync(int userId);
        Task<IEnumerable<RoleAgreementByUser>> getOldRolAgreementByUserId(int userId);
        Task RemoveRoleAgreement(object item);
        Task AddRoleAgreement(RoleAgreementByUser roleAgreementByUser);
        Task UpdateRoleAgreement(object item);

        Task UpdateUserRoles(int userId, IEnumerable<long> roleIds, long updaterUserId);
    }

    public class UserRoleAgreementNewDto
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int AgreementId { get; set; }
        public string AgreementName { get; set; } = string.Empty;
        public long RoleAgreementByUserId { get; set; }
    }
}
