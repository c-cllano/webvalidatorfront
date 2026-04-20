using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Roles.Delete
{
    public record DeleteRoleCommand(long RoleId) : IRequest<SsoServiceResult<long>>;
}
