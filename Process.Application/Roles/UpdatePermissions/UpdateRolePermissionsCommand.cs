using MediatR;  
using Process.Domain.Entities;

namespace Process.Application.Roles.UpdatePermissions
{
    public record UpdateRolePermissionsCommand(long RoleId, List<PermissionSelection> PermissionIds)
        : IRequest<SsoServiceResult<long>>;
}
