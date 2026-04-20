using MediatR;
using Process.Application.RoleByAgreement.Dto;
using Process.Domain.Entities;

namespace Process.Application.RoleByAgreement.Create
{
    public record CreateRoleByAgreementCommand(long RoleId, int AgreementId, long CreatorUserId) 
        : IRequest<SsoServiceResult<long>>;
}
