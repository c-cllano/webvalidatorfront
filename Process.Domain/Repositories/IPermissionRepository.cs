using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetPermissionsByMenuIds(IEnumerable<long> menuIds);
        Task<IEnumerable<Permission>> GetAllPermissionsWithMenu();
    }
}
