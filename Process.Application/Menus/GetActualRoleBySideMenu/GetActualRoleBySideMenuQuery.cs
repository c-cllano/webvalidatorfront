using MediatR;

namespace Process.Application.Menus.GetActualRoleBySideMenu
{

    public record GetActualRoleBySideMenuQuery(long UserId , long AgreementId) : IRequest<List<Process.Domain.Entities.RoleActual>>;
}
