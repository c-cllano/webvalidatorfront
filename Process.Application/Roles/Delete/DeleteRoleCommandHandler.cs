using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Roles.Delete
{
    public class DeleteRoleCommandHandler(IRoleRepository roleRepository) : IRequestHandler<DeleteRoleCommand, SsoServiceResult<long>>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        public async Task<SsoServiceResult<long>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRoleById(request.RoleId);
            if (role is null)
                return SsoServiceResult<long>.Fail("El rol no existe", 404);
           
            if (role.IsDeleted)
                return SsoServiceResult<long>.Fail("El rol ya fue eliminado", 400);
             

            role.IsDeleted = true;
            role.UpdatedDate = DateTime.UtcNow;

            await _roleRepository.DeleteRole(role.RoleId);

            return SsoServiceResult<long>.Ok(role.RoleId);
        }
    } 
}
