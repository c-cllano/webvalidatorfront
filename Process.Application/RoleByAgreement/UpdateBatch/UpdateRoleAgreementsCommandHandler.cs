using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.RoleByAgreement.UpdateBatch
{
    public class UpdateRoleAgreementsCommandHandler(
        IRoleByAgreementRepository roleByAgreementRepository,
        IRoleRepository roleRepository) 
        : IRequestHandler<UpdateRoleAgreementsCommand, SsoServiceResult<bool>>
    {
        private readonly IRoleByAgreementRepository _roleByAgreementRepository = roleByAgreementRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<SsoServiceResult<bool>> Handle(
            UpdateRoleAgreementsCommand request, 
            CancellationToken cancellationToken)
        {
            // Validar que el rol exista
            var role = await _roleRepository.GetRoleById(request.RoleId);
            if (role == null)
                return SsoServiceResult<bool>.Fail("El rol no existe", 404);

            // Actualizar los agreements del rol
            await _roleByAgreementRepository.UpdateRoleAgreements(
                request.RoleId, 
                request.AgreementIds, 
                request.UpdaterUserId);

            return SsoServiceResult<bool>.Ok(true);
        }
    }
}
