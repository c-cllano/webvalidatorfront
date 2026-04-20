using MediatR;
using Process.Application.RoleByAgreement.Dto;
using Process.Domain.Entities;

namespace Process.Application.RoleByAgreement.GetAll
{
    public record GetAllRoleByAgreementQuery : IRequest<SsoServiceResult<List<RoleByAgreementDto>>>;
}
