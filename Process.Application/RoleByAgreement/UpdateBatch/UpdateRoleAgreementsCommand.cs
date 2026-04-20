using MediatR;
using Process.Domain.Entities;

namespace Process.Application.RoleByAgreement.UpdateBatch
{
    public record UpdateRoleAgreementsCommand(long RoleId, List<int> AgreementIds, long UpdaterUserId) 
        : IRequest<SsoServiceResult<bool>>;
}
