using Dactyloscopy.Domain.Parameters.ExternalApiClientParameters;
using MediatR;

namespace Dactyloscopy.Application.RequestForensicReview
{
    public record RequestForensicReviewCommand(
        long ValidationProcessId,
        Guid AgreementGuid,
        int DocumentTypeId,
        string DocumentNumber,
        IEnumerable<ForensicObjectRequest> Objects
    ) : IRequest<RequestForensicReviewResponse>;
}
