using Dactyloscopy.Domain.Entities;
using Dactyloscopy.Domain.Repositories;
using Dactyloscopy.Domain.Utilities;
using MediatR;

namespace Dactyloscopy.Application.GetForensicReviewProcessInReview
{
    public class GetForensicReviewProcessInReviewQueryHandler(
        IForensicReviewProcessRepository forensicReviewProcessRepository,
        IStatusForensicRepository statusForensicRepository
    ) : IRequestHandler<GetForensicReviewProcessInReviewQuery, IEnumerable<GetForensicReviewProcessResponse>>
    {
        private readonly IForensicReviewProcessRepository _forensicReviewProcessRepository = forensicReviewProcessRepository;
        private readonly IStatusForensicRepository _statusForensicRepository = statusForensicRepository;

        public async Task<IEnumerable<GetForensicReviewProcessResponse>> Handle(GetForensicReviewProcessInReviewQuery request, CancellationToken cancellationToken)
        {
            StatusForensic? statusForensic = await _statusForensicRepository
                .GetStatusByDescriptionAsync(Constants.InReview)
                    ?? throw new KeyNotFoundException($"No se encontró estado forense 'En revisión'");

            IEnumerable<ForensicReviewProcess> listForensicReviewProcess = await _forensicReviewProcessRepository
                .GetForensicReviewProcessInReviewAsync(statusForensic.StatusForensicId);

            IEnumerable<GetForensicReviewProcessResponse> response = listForensicReviewProcess?
                .Select(x => new GetForensicReviewProcessResponse
                {
                    ForensicReviewProcessId = x.ForensicReviewProcessId,
                    ValidationProcessId = x.ValidationProcessId,
                    TxGuidForense = x.TxGuidForense,
                    StatusForensicId = x.StatusForensicId,
                    VerificationDate = x.VerificationDate,
                    Approved = x.Approved,
                    Score = x.Score,
                    MainReason = x.MainReason,
                    OptionalReason = x.OptionalReason,
                    Description = x.Description,
                    Observation = x.Observation
                }) ?? [];

            return response;
        }
    }
}
