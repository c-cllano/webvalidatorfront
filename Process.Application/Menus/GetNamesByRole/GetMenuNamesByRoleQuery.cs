using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Menus.GetNamesByRole
{
    public record GetMenuNamesByRoleQuery(long RoleId) : IRequest<SsoServiceResult<List<string>>>;
}
