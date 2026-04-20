using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Roles.Create
{
    public record CreateRoleCommand(
        string Name,
        bool Active,
        Guid ClientGuid,
        int CreatorUserId,
        List<PermissionSelection> Permissions
        ) : IRequest<SsoServiceResult<long>>;
}
