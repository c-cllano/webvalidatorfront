using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Roles.GetByUser
{

    public record GetRoleByUserQuery(long UserId, long? agreementId) : IRequest<List<Process.Domain.Entities.RoleByUserNew>>;
}
