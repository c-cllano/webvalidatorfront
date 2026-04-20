using MediatR;
using Process.Domain.Repositories;

namespace Process.Application.ParametersAgreement.GetById
{
    public class GetParametersAgreementByIdQueryHandler(
        IParametersAgreementRepository repository
    ) : IRequestHandler<GetParametersAgreementByIdQuery, IEnumerable<GetParametersAgreementByIdQueryResponse>>
    {

        public async Task<IEnumerable<GetParametersAgreementByIdQueryResponse>> Handle(GetParametersAgreementByIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = await repository
                .GetParametersAgreementByAgreementGuidAsync(request.AgreementGuid, request.ParameterCode);

            return parameters
                .Select(
                    e => new GetParametersAgreementByIdQueryResponse(
                        e.ParameterAgreementId,
                        e.AgreementId,
                        e.ParameterAgreementGuid,
                        e.ParameterName,
                        e.ParameterValue,
                        e.CreatedDate,
                        e.UpdatedDate,
                        e.Active,
                        e.IsDeleted,
                        e.ParameterCode,
                        e.ParameterDescription
                    )
                );
        }
    }
}
