using System;
using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.ParametersAgreement.GetAll
{
    public class GetParametersAgreementQueryHandler(IParametersAgreementRepository repository) : 
        IRequestHandler<GetParametersAgreementQuery, 
            IEnumerable<GetParametersAgreementQueryResponse>>
    {
        public async Task<IEnumerable<GetParametersAgreementQueryResponse>> Handle(GetParametersAgreementQuery request, CancellationToken cancellationToken)
        {

            var result  = await repository.GetAllAsync();

            return result.Select(s => new GetParametersAgreementQueryResponse(
                s.ParameterAgreementId,
                s.AgreementId,
                s.ParameterAgreementGuid,
                s.ParameterName,
                s.ParameterValue,
                s.CreatedDate,
                s.UpdatedDate,
                s.Active,
                s.IsDeleted
                ));
        }
    }
}
