using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IMenuRepository
    { 
        Task<IEnumerable<Menu>> GetAllAsync();
        Task<IEnumerable<Menu>> GetMenusByRoleAsync(long roleId);
        Task<IEnumerable<string>> GetLeafMenuNamesByRoleAsync(long roleId);
        Task<IEnumerable<MenuWithPermissionsDto>> GetMenusWithPermissionsAsync(long? roleId);
        Task<IEnumerable<Menu>> GetMenusByAgreementAndUserAsync(long agreementId, long userId);
        Task<List<Entities.RoleActual>> GetActualRoleBySideMenu(long userId, long agreementId);
    }

    public class MenuWithPermissionsDto
    {
        public long MenuId { get; set; }
        public long? ParentId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool Active { get; set; }
        public bool MenuSelected { get; set; }
        public long? PermissionId { get; set; }
        public string? PermissionCode { get; set; }
        public string? PermissionName { get; set; }
        public int? PermissionOrder { get; set; }
        public bool PermissionSelected { get; set; }

        public bool Visible { get; set; }
    }
}
