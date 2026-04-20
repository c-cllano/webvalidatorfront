using MediatR;
using Process.Application.RoleByAgreement.Dto;
using Process.Domain.Entities;

namespace Process.Application.RoleByAgreement.GetByRole
{
    public record GetRoleByAgreementByRoleQuery(long RoleId) 
        : IRequest<SsoServiceResult<List<RoleByAgreementDto>>>;
}
