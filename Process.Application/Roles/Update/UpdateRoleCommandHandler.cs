using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Roles.Update
{
    public class UpdateRoleCommandHandler(IRoleRepository roleRepository) : IRequestHandler<UpdateRoleCommand, SsoServiceResult<long>>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;  

        public async Task<SsoServiceResult<long>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRoleById(request.RoleId);
            if (role is null)
                return SsoServiceResult<long>.Fail("El rol no existe", 404);

            var roleExists = await _roleRepository.GetRoleByName(request.Name, role.ClientId);
            if (roleExists is not null && roleExists.RoleId != request.RoleId)
                return SsoServiceResult<long>.Fail("El nombre del rol ya existe", 400);

            role.Name = request.Name;
            role.Active = request.Active;
            role.UpdatedDate = DateTime.UtcNow;
            await _roleRepository.UpdateRole(role); 

            return SsoServiceResult<long>.Ok(role.RoleId);
        }
    }
} 