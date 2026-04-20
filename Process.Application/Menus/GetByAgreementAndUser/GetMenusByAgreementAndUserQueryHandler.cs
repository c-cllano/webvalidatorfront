using MediatR;
using Process.Application.Menus.GetAll;
using Process.Application.Menus.Mappers;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Menus.GetByAgreementAndUser
{


    public class GetMenusByAgreementAndUserQueryHandler(IMenuRepository repository) : IRequestHandler<GetMenusByAgreementAndUserQuery, SsoServiceResult<List<MenuResponse>>>
    {
        private readonly IMenuRepository _repository = repository;
        public async Task<SsoServiceResult<List<MenuResponse>>> Handle(GetMenusByAgreementAndUserQuery request, CancellationToken cancellationToken)
        {
            var menus = await _repository.GetMenusByAgreementAndUserAsync(request.agreementId, request.userId);
            var menuTree = MenuMapper.BuildMenuTree(menus);
            return SsoServiceResult<List<MenuResponse>>.Ok(menuTree);
        }
    }
}
