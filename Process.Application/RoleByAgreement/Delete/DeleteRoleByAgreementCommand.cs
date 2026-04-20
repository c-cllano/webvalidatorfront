using MediatR;
using Process.Domain.Entities;

namespace Process.Application.RoleByAgreement.Delete
{
    public record DeleteRoleByAgreementCommand(long RoleByAgreementId) 
        : IRequest<SsoServiceResult<long>>;
}
