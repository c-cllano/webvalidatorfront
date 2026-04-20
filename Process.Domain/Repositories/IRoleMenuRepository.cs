namespace Process.Domain.Repositories
{
    public interface IRoleMenuRepository
    {
        Task<int> DeleteRoleMenu(long roleMenuId);

        Task<int> InsertRoleMenus(long roleId, IEnumerable<long> menuIds, long creatorUserId);

        Task UpdateRoleMenus(long roleId, IEnumerable<long> menuIds, long updaterUserId); 
        Task<IEnumerable<long>> GetMenuIdsByRoleId(long roleId);
    }
}
