using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.RoleByAgreement.Delete
{
    public class DeleteRoleByAgreementCommandHandler(IRoleByAgreementRepository repository) 
        : IRequestHandler<DeleteRoleByAgreementCommand, SsoServiceResult<long>>
    {
        private readonly IRoleByAgreementRepository _repository = repository;

        public async Task<SsoServiceResult<long>> Handle(
            DeleteRoleByAgreementCommand request, 
            CancellationToken cancellationToken)
        {
            var roleByAgreement = await _repository.GetByIdAsync(request.RoleByAgreementId);
            
            if (roleByAgreement == null)
                return SsoServiceResult<long>.Fail("La relación rol-convenio no existe", 404);

            if (roleByAgreement.IsDeleted)
                return SsoServiceResult<long>.Fail("La relación ya fue eliminada", 400);

            await _repository.DeleteAsync(request.RoleByAgreementId);

            return SsoServiceResult<long>.Ok(request.RoleByAgreementId);
        }
    }
}
