using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Sso.User
{
    public class UpdateUserAssignmentsHandler(
        IRoleAgreementByUserRepository roleAgreementByUserRepository) : IRequestHandler<UpdateUserAssignmentsQuery, object>
    {
        private readonly IRoleAgreementByUserRepository _roleAgreementByUserRepository = roleAgreementByUserRepository;

        public async Task<object> Handle(UpdateUserAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var oldAgreements = await _roleAgreementByUserRepository.getOldRolAgreementByUserId(request.UserId);


            var requestAgreements = request.AssignmentRoleAgreements;

            // 1. Identificar los que se mantienen o deben REACTIVARSE (están en request Y en la base de datos)
            var toUpdate = oldAgreements
                .Where(old => requestAgreements.Any(r => r.RoleId == old.RoleId && r.AgreementId == old.AgreementId))
                .ToList();

            // 2. Identificar los que realmente se van (están en BD pero NO en el request)
            var toRemove = oldAgreements
                .Where(old => !requestAgreements.Any(r => r.RoleId == old.RoleId && r.AgreementId == old.AgreementId))
                .ToList();

            // 3. Identificar los nuevos (están en request pero NO en la base de datos)
            var toAdd = requestAgreements
                .Where(r => !oldAgreements.Any(old => old.RoleId == r.RoleId && old.AgreementId == r.AgreementId))
                .ToList();

            // ACCIONES:

            // Reactivar si estaban inactivos
            foreach (var item in toUpdate)
            {
                item.Active = true;
                item.IsDeleted = false;
                await _roleAgreementByUserRepository.UpdateRoleAgreement(item);
            }

            // Eliminar (Baja lógica o física según tu repo)
            foreach (var item in toRemove)
            {
                await _roleAgreementByUserRepository.RemoveRoleAgreement(item);
            }

            // Insertar nuevos
            foreach (var item in toAdd)
            {
                await _roleAgreementByUserRepository.AddRoleAgreement(new RoleAgreementByUser
                {
                    UserId = request.UserId,
                    RoleId = item.RoleId,
                    AgreementId = item.AgreementId,
                    CreatedDate = DateTime.UtcNow,
                    CreatorUserId = request.UpdaterUserId,
                    Active = true,
                    IsDeleted = false
                });
            }

            return new { Success = true };
        }

    }
}
