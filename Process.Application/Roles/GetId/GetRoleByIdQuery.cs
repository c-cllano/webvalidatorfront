using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Roles.GetId
{
    public record GetRoleByIdQuery(long Id) : IRequest<SsoServiceResult<RoleByIdResponse>>;
}
