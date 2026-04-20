using MediatR;
using Process.Domain.Repositories;


namespace Process.Application.Roles.GetByUser
{

    public class GetRoleByUserQueryHandler(IRolePermissionRepository repository) : IRequestHandler<GetRoleByUserQuery, List<Domain.Entities.RoleByUserNew> >

    {

        private readonly IRolePermissionRepository _rolePermissionRepository = repository;

        public async Task<List<Domain.Entities.RoleByUserNew>> Handle(GetRoleByUserQuery request, CancellationToken cancellationToken)
        {
            var usersRole = await _rolePermissionRepository.GetRoleByUser(request.UserId, request.agreementId);
            return (List<Domain.Entities.RoleByUserNew>)usersRole;
        }

    }

}