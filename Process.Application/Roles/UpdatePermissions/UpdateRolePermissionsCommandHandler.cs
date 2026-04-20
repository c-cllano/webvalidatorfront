using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Roles.UpdatePermissions
{
    public class UpdateRolePermissionsCommandHandler(
        IRoleRepository roleRepository,
        IRolePermissionRepository rolePermissionRepository,
        IRoleMenuRepository roleMenuRepository
    ) : IRequestHandler<UpdateRolePermissionsCommand, SsoServiceResult<long>>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository = rolePermissionRepository;
        private readonly IRoleMenuRepository _roleMenuRepository = roleMenuRepository;

        public async Task<SsoServiceResult<long>> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRoleById(request.RoleId);
            if (role is null)
                return SsoServiceResult<long>.Fail("El rol no existe", 404);

            var selections = request.PermissionIds ?? new List<Process.Application.Roles.PermissionSelection>();

            var menuIds = selections
                .Select(x => x.MenuId)
                .Distinct()
                .ToList();

            if (menuIds.Count > 0)
            {
                await _roleMenuRepository.UpdateRoleMenus(role.RoleId, menuIds, role.CreatorUserId);
            }

            var permissionIds = selections
                .Select(x => x.PermissionId)
                .Distinct()
                .ToList();
            if (permissionIds.Count > 0)
            {
                await _rolePermissionRepository.UpsertRolePermissions(
                    role.RoleId,
                    permissionIds,
                    role.CreatorUserId
                );
            }


            return SsoServiceResult<long>.Ok(role.RoleId);
        }
    }
}
