using MediatR;
using Process.Domain.Repositories;
using Process.Domain.Entities;

namespace Process.Application.Roles.Create
{
    public class CreateRoleCommandHandler(
        IRoleRepository repository, 
        IClientRepository clientRepository,
        IRolePermissionRepository rolePermissionRepository,
        IRoleMenuRepository roleMenuRepository) : IRequestHandler<CreateRoleCommand, SsoServiceResult<long>>
    {
        private readonly IRoleRepository _repository = repository;
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository = rolePermissionRepository;
        private readonly IRoleMenuRepository _roleMenuRepository = roleMenuRepository;

        public async Task<SsoServiceResult<long>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByToken(request.ClientGuid);
            if (client is null)
                return SsoServiceResult<long>.Fail("El cliente no existe", 404);

            var roleExists = await _repository.GetRoleByName(request.Name, client.ClientId);
            if (roleExists is not null)
                return SsoServiceResult<long>.Fail("El nombre del rol ya existe", 400);

            var role = new Role
            {
                Name = request.Name,
                ClientId = client.ClientId,
                Active = request.Active,
                CreatedDate = DateTime.UtcNow,
                CreatorUserId = request.CreatorUserId,
                IsDeleted = false
            };

            var roleId = await _repository.InsertRole(role);

            if (request.Permissions is { Count: > 0 })
            {

                var menuIds = request.Permissions
                    .Select(x => x.MenuId)
                    .Distinct()
                    .ToList();
                if (menuIds.Count > 0)
                {
                    await _roleMenuRepository.InsertRoleMenus(roleId, menuIds, request.CreatorUserId);
                }

                var permissionIds = request.Permissions
                    .Where(s => s.PermissionId > 0)
                    .Select(x => x.PermissionId )
                    .Distinct()
                    .ToList();
                await _rolePermissionRepository.UpsertRolePermissions(roleId, permissionIds, request.CreatorUserId);

              
            }

            return SsoServiceResult<long>.Ok(roleId);
        }
    }
}
