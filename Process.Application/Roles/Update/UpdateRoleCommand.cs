using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Roles.Update
{
    public record UpdateRoleCommand(
        long RoleId, 
        string Name,
        bool Active)
        : IRequest<SsoServiceResult<long>>;
     
}
