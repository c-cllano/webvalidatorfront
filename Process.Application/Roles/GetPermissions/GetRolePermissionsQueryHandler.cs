using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Roles.GetPermissions
{
    public class GetRolePermissionsQueryHandler(IRolePermissionRepository rolePermissionRepository)
        : IRequestHandler<GetRolePermissionsQuery, SsoServiceResult<List<long>>>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository = rolePermissionRepository;

        public async Task<SsoServiceResult<List<long>>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var ids = await _rolePermissionRepository.GetPermissionIdsByRoleId(request.RoleId);
            return SsoServiceResult<List<long>>.Ok(ids?.ToList() ?? new List<long>());
        }
    }
}
