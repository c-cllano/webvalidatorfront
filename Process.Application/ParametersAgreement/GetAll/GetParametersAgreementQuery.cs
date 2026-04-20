using MediatR;

namespace Process.Application.ParametersAgreement.GetAll
{
    public class GetParametersAgreementQuery : IRequest<IEnumerable<GetParametersAgreementQueryResponse>> { }
}
