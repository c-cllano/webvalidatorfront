using MediatR;

namespace Dactyloscopy.Application.GetForensicReviewProcessInReview
{
    public record GetForensicReviewProcessInReviewQuery : IRequest<IEnumerable<GetForensicReviewProcessResponse>>;
}
