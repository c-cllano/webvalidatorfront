using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Menus.GetNamesByRole
{
    public class GetMenuNamesByRoleQueryHandler(IMenuRepository menuRepository)
        : IRequestHandler<GetMenuNamesByRoleQuery, SsoServiceResult<List<string>>>
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        public async Task<SsoServiceResult<List<string>>> Handle(GetMenuNamesByRoleQuery request, CancellationToken cancellationToken)
        {
            var names = (await _menuRepository.GetLeafMenuNamesByRoleAsync(request.RoleId)).ToList();
            return SsoServiceResult<List<string>>.Ok(names);
        }
    }
}
