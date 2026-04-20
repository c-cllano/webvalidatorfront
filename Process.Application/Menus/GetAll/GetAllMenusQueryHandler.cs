using MediatR;
using Process.Application.Menus.Mappers;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Menus.GetAll
{
    public class GetAllMenusQueryHandler(IMenuRepository repository) : IRequestHandler<GetAllMenusQuery, SsoServiceResult<List<MenuResponse>>>
    {
        private readonly IMenuRepository _repository = repository;
        public async Task<SsoServiceResult<List<MenuResponse>>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
        {
            var menus = await _repository.GetAllAsync();
            var menuTree = MenuMapper.BuildMenuTree(menus);
            return SsoServiceResult<List<MenuResponse>>.Ok(menuTree);
        }
    }
    
}
