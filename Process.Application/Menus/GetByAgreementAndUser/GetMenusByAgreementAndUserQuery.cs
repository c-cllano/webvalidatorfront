using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Menus.GetByAgreementAndUser
{

    public record GetMenusByAgreementAndUserQuery(long agreementId, long userId) : IRequest<SsoServiceResult<List<MenuResponse>>>;
}
