using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Menus.GetWithPermissions
{
    public class GetMenusWithPermissionsQueryHandler(IMenuRepository menuRepository)
        : IRequestHandler<GetMenusWithPermissionsQuery, SsoServiceResult<List<MenuWithPermissionsResponse>>>
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        public async Task<SsoServiceResult<List<MenuWithPermissionsResponse>>> Handle(GetMenusWithPermissionsQuery request,
            CancellationToken cancellationToken)
        {
            var data = (await _menuRepository.GetMenusWithPermissionsAsync(request.RoleId)).ToList();
            
            if (data.Count == 0)
                return SsoServiceResult<List<MenuWithPermissionsResponse>>.Ok([]);

          
            var response = data
                .GroupBy(d => new 
                { 
                    d.MenuId, 
                    d.ParentId, 
                    d.Order, 
                    d.Title, 
                    d.Description, 
                    d.Icon, 
                    d.Link, 
                    d.Active, 
                    d.MenuSelected 
                })
                .Select(g => new MenuWithPermissionsResponse
                {
                    MenuId = g.Key.MenuId,
                    ParentId = g.Key.ParentId,
                    Order = g.Key.Order,
                    Title = g.Key.Title,
                    Description = g.Key.Description,
                    Icon = g.Key.Icon,
                    Link = g.Key.Link,
                    Active = g.Key.Active,
                    Selected = g.Key.MenuSelected,
                    Permissions = g
                        .Where(d => d.PermissionId.HasValue)
                        .Select(d => new PermissionItem
                        {
                            PermissionId = d.PermissionId!.Value,
                            Code = d.PermissionCode ?? string.Empty,
                            Name = d.PermissionName ?? string.Empty,
                            Order = d.PermissionOrder ?? 0,
                            Selected = d.PermissionSelected
                        })
                        .OrderBy(p => p.Order)
                        .ToList()
                })
                .OrderBy(m => m.Order)
                .ToList();

            return SsoServiceResult<List<MenuWithPermissionsResponse>>.Ok(response);
        }
    }
}
