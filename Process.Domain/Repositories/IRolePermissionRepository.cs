using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IRolePermissionRepository
    {
        Task<IEnumerable<long>> GetPermissionIdsByRoleId(long roleId);
        Task<List<Process.Domain.Entities.RoleByUserNew>> GetRoleByUser(long userId, long? agreementId);
        Task UpsertRolePermissions(long roleId, IEnumerable<long> permissionIds, long userId);
    }
}
