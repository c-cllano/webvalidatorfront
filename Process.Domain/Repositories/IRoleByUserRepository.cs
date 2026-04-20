using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IRoleByUserRepository
    {
        Task<long> AddAsync(RoleByUser roleByUser);
        Task<IEnumerable<RoleByUser>> GetByUserIdAsync(int userId);
        Task<RoleByUser?> GetByUserIdAndRoleIdAsync(int userId, long roleId);
        Task UpdateUserRoles(int userId, IEnumerable<long> roleIds, long updaterUserId);
        Task<IEnumerable<UserRoleAgreementDto>> GetUserRolesAndAgreementsAsync(int userId);
    }

    /// <summary>
    /// DTO para la consulta optimizada de roles y agreements del usuario
    /// </summary>
    public class UserRoleAgreementDto
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int AgreementId { get; set; }
        public string AgreementName { get; set; } = string.Empty;
        public long RoleByAgreementId { get; set; }
    }
}
