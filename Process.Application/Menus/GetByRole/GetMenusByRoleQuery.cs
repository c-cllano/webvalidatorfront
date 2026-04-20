using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Menus.GetByRole
{    public record GetMenusByRoleQuery(long RoleId) : IRequest<SsoServiceResult<List<MenuResponse>>>;
}
