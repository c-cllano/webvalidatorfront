using MediatR;
using Process.Application.Menus.Mappers;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Menus.GetByRole
{
    public class GetMenusByRoleQueryHandler(IMenuRepository repository) : IRequestHandler<GetMenusByRoleQuery, SsoServiceResult<List<MenuResponse>>>
    {
        private readonly IMenuRepository _repository = repository;
        public async Task<SsoServiceResult<List<MenuResponse>>> Handle(GetMenusByRoleQuery request, CancellationToken cancellationToken)
        {
            var menus = await _repository.GetMenusByRoleAsync(request.RoleId);

            var menuTree = MenuMapper.BuildMenuTree(menus);

            return SsoServiceResult<List<MenuResponse>>.Ok(menuTree);
        }
    }
}
