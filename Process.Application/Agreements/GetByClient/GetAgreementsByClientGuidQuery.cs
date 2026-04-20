using MediatR;

namespace Process.Application.Agreements.GetByClient
{
    public record GetAgreementsByClientGuidQuery(Guid Guid) : IRequest<IReadOnlyList<GetAgreementsByClientGuidQueryResponse>>;
}
