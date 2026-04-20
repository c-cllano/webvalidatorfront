using MediatR;

namespace Process.Application.ParametersAgreement.GetById
{
    public record GetParametersAgreementByIdQuery(
        Guid AgreementGuid,
       IEnumerable<string>? ParameterCode = null
    ) : IRequest<IEnumerable<GetParametersAgreementByIdQueryResponse>>;
}
