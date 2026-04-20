using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IRoleRepository
    {
         Task<int> GetUserCountByRoleId(long roleId);
        Task<IEnumerable<RoleDto>> GetAllRoles(Guid clientGuid, string[]? rolName, string? status, int? pageNumber, int? pageSize);
        Task<int> DeleteRole(long roleId);
        Task<int> InsertRole(Role role);
        Task<int> UpdateRole(Role role);
        Task<Role?> GetRoleById(long roleId);
        Task<Role?> GetRoleByName(string name, long clientId);
    }
}
