using MediatR;
using Process.Application.AgreementByUser.GetByUserId;
using Process.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Application.AgreementByUser.GetByGuid
{

    public class GetAgreementByUserByGuidQueryHandler(IAgreementByGuidRepository agreementByGuidRepository) :
        IRequestHandler<GetAgreementByUserByGuidQuery, IEnumerable<GetByGuidQueryResponse>>
    {
        public async Task<IEnumerable<GetByGuidQueryResponse>> Handle(GetAgreementByUserByGuidQuery request, CancellationToken cancellationToken)
        {
            var agreements = await agreementByGuidRepository.GetByGuidAsync(request.Guid);
            return agreements.Select(a => new GetByGuidQueryResponse
            {

                Name = a.Name,
                AgreementGuid = a.AgreementGuid,
                ClientId = a.ClientId,
                UserReconoserId = a.UserReconoserId,
                AgreementId = a.AgreementId
            });

        }
    }
}



