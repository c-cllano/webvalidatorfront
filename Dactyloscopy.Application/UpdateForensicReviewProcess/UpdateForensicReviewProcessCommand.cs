using MediatR;

namespace Dactyloscopy.Application.UpdateForensicReviewProcess
{
    public record UpdateForensicReviewProcessCommand(
        long ForensicReviewProcessId,
        long ValidationProcessId,
        Guid TxGuidForense,
        long StatusForensicId,
        DateTime? VerificationDate,
        bool? Approved,
        decimal? Score,
        string? MainReason,
        string? OptionalReason,
        string? Description,
        string? Observation
    ) : IRequest;
}
