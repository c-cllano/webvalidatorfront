using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Roles.GetPermissions
{
    public record GetRolePermissionsQuery(long RoleId) : IRequest<SsoServiceResult<List<long>>>;
}
