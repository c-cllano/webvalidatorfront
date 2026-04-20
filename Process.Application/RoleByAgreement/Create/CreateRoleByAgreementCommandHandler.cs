using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.RoleByAgreement.Create
{
    public class CreateRoleByAgreementCommandHandler(
        IRoleByAgreementRepository roleByAgreementRepository,
        IRoleRepository roleRepository) 
        : IRequestHandler<CreateRoleByAgreementCommand, SsoServiceResult<long>>
    {
        private readonly IRoleByAgreementRepository _roleByAgreementRepository = roleByAgreementRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<SsoServiceResult<long>> Handle(CreateRoleByAgreementCommand request, CancellationToken cancellationToken)
        {
            // Validar que el rol exista
            var role = await _roleRepository.GetRoleById(request.RoleId);
            if (role == null)
                return SsoServiceResult<long>.Fail("El rol no existe", 404);

            // Validar que no exista ya la relación
            var exists = await _roleByAgreementRepository.ExistsAsync(request.RoleId, request.AgreementId);
            if (exists)
                return SsoServiceResult<long>.Fail("La relación entre el rol y el convenio ya existe", 400);

            // Crear la relación
            var roleByAgreement = new Domain.Entities.RoleByAgreement
            {
                RoleId = request.RoleId,
                AgreementId = request.AgreementId,
                CreatorUserId = request.CreatorUserId,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                Active = true,
                IsDeleted = false
            };

            var id = await _roleByAgreementRepository.AddAsync(roleByAgreement);
            return SsoServiceResult<long>.Ok(id);
        }
    }
}
