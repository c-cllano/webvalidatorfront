using Domain.Enums;
using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Utilities;

namespace Process.Application.Roles.GetId
{
    public class GetRoleByIdHandler(IRoleRepository role) : IRequestHandler<GetRoleByIdQuery, SsoServiceResult<RoleByIdResponse>>
    {
        public async Task<SsoServiceResult<RoleByIdResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await role.GetRoleById(request.Id);
            if (result is null)
                return SsoServiceResult<RoleByIdResponse>.Fail("El rol no existe", 404);

            var userCount = await role.GetUserCountByRoleId(request.Id);

            var roleDto = new RoleByIdResponse
            {
                RoleId = result.RoleId,
                Name = result.Name, 
                Active = result.Active, 
                Status = result.Active ? 
                Constants.StatusActive : Constants.StatusInactive,
                Users = userCount
            };
            return SsoServiceResult<RoleByIdResponse>.Ok(roleDto);
        }
    }
}
