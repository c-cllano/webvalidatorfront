using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Menus.GetWithPermissions
{
    public record GetMenusWithPermissionsQuery(long? RoleId) : IRequest<SsoServiceResult<List<MenuWithPermissionsResponse>>>;

    public class MenuWithPermissionsResponse
    {
        public long MenuId { get; set; }
        public long? ParentId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool Active { get; set; }
        public bool Selected { get; set; }

        public List<PermissionItem> Permissions { get; set; } = new();
    }

    public class PermissionItem
    {
        public long PermissionId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool Selected { get; set; }

    }
}
