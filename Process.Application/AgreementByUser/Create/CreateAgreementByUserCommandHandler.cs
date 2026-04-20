using MediatR;
using Process.Domain.Repositories;

namespace Process.Application.AgreementByUser.Create
{
    public class CreateAgreementByUserCommandHandler(IAgreementByUserRepository repository) : IRequestHandler<CreateAgreementByUserCommand, long>
    {
        public async Task<long> Handle(CreateAgreementByUserCommand request, CancellationToken cancellationToken)
        {
            var agreementByUser = new Domain.Entities.AgreementByUser
            {
                UserId = request.UserId,
                AgreementId = request.AgreementId,
                CreatedUserId = request.CreatedUserId,
                CreatedDate = request.CreatedDate ?? DateTime.UtcNow,
                Active = request.Active,
                IsDelete = false
            };
            var id = await repository.AddAsync(agreementByUser);
            return id;
        }
    }
}
