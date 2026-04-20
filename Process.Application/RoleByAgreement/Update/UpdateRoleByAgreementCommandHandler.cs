using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.RoleByAgreement.Update
{
    public class UpdateRoleByAgreementCommandHandler(IRoleByAgreementRepository repository) 
        : IRequestHandler<UpdateRoleByAgreementCommand, SsoServiceResult<long>>
    {
        private readonly IRoleByAgreementRepository _repository = repository;

        public async Task<SsoServiceResult<long>> Handle(
            UpdateRoleByAgreementCommand request, 
            CancellationToken cancellationToken)
        {
            var roleByAgreement = await _repository.GetByIdAsync(request.RoleByAgreementId);
            
            if (roleByAgreement == null)
                return SsoServiceResult<long>.Fail("La relación rol-convenio no existe", 404);

            roleByAgreement.Active = request.Active;
            roleByAgreement.UpdatedDate = DateTime.UtcNow.AddHours(-5);

            await _repository.UpdateAsync(roleByAgreement);

            return SsoServiceResult<long>.Ok(roleByAgreement.RoleByAgreementId);
        }
    }
}
