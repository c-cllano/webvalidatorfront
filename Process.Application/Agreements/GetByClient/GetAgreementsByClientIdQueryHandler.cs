using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.Agreements.GetByClient
{
    public class GetAgreementsByClientIdQueryHandler(IAgreementOkeyStudioRepository repository) :
            IRequestHandler<GetAgreementsByClientGuidQuery,
                IReadOnlyList<GetAgreementsByClientGuidQueryResponse>>
    {

        public async Task<IReadOnlyList<GetAgreementsByClientGuidQueryResponse>> Handle(
        GetAgreementsByClientGuidQuery request, CancellationToken cancellationToken)
        {
            var agreementList = await repository.GetAgreementsByClientId(request.Guid);
            return [.. agreementList.Select(MapToDto)];
        }

        private static GetAgreementsByClientGuidQueryResponse MapToDto(AgreementOkeyStudio a)
            => new(a.AgreementId, a.AgreementGUID, a.Name ?? string.Empty);
    }
}
