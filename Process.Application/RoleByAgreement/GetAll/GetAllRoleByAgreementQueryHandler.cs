using MediatR;
using Process.Application.RoleByAgreement.Dto;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.RoleByAgreement.GetAll
{
    public class GetAllRoleByAgreementQueryHandler(IRoleByAgreementRepository repository) 
        : IRequestHandler<GetAllRoleByAgreementQuery, SsoServiceResult<List<RoleByAgreementDto>>>
    {
        private readonly IRoleByAgreementRepository _repository = repository;

        public async Task<SsoServiceResult<List<RoleByAgreementDto>>> Handle(
            GetAllRoleByAgreementQuery request, 
            CancellationToken cancellationToken)
        {
            var roleByAgreements = await _repository.GetAllAsync();
            
            var dtos = roleByAgreements.Select(rba => new RoleByAgreementDto
            {
                RoleByAgreementId = rba.RoleByAgreementId,
                RoleId = rba.RoleId,
                RoleName = rba.RoleName ?? string.Empty,
                AgreementId = rba.AgreementId,
                AgreementName = rba.AgreementName ?? string.Empty,
                Active = rba.Active,
                CreatedDate = rba.CreatedDate,
                UpdatedDate = rba.UpdatedDate
            }).ToList();

            return SsoServiceResult<List<RoleByAgreementDto>>.Ok(dtos);
        }
    }
}
