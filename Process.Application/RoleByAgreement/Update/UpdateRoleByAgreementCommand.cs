using MediatR;
using Process.Domain.Entities;

namespace Process.Application.RoleByAgreement.Update
{
    public record UpdateRoleByAgreementCommand(long RoleByAgreementId, bool Active) 
        : IRequest<SsoServiceResult<long>>;
}
