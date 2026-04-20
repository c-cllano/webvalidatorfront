using MediatR;
using Process.Domain.Repositories;
using System.Data;

namespace Process.Application.AgreementByUser.GetByUserId
{
    public class GetAgreementByUserByUserIdQueryHandler(IAgreementByUserRepository agreementByUserRepository) :
        IRequestHandler<GetAgreementByUserByUserIdQuery, IEnumerable<GetByUserIdQueryResponse>>
    {
        public async Task<IEnumerable<GetByUserIdQueryResponse>> Handle(GetAgreementByUserByUserIdQuery request, CancellationToken cancellationToken)
        {
            var agreements = await agreementByUserRepository.GetByUserIdAsync(request.UserId);
            return agreements.Select(a => new GetByUserIdQueryResponse
            {
                AgreementName = a.AgreementName,
                AgreementGuid = a.AgreementGUID,
                Active = a.Active,
                AgreementByUserId = a.AgreementByUserId,
                AgreementId = a.AgreementId,
                ClientToken = a.clientToken
            });

        }
    }
}