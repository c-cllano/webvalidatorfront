using MediatR;
using Process.Domain.Repositories;

namespace Process.Application.Roles.GetByClient
{
    public class GetRoleByClientHandler(IRoleRepository repository) : IRequestHandler<GetRoleByClientQuery, List<GetRoleByClientResponse>>
    {
        private readonly IRoleRepository _repository = repository;
        public async Task<List<GetRoleByClientResponse>> Handle(GetRoleByClientQuery request, CancellationToken cancellationToken)
        {
            var roles = await _repository.GetAllRoles(
                request.ClientGuid,
                request.RolName,
                request.Status, 
                request.PageNumber,
                request.PageSize);

            return [.. roles.Select(r => new GetRoleByClientResponse(r.Id, r.Name, r.Status, r.Users, r.ClientId))];
        }
    }
}
