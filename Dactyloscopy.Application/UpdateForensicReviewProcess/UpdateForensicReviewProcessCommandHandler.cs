using Dactyloscopy.Domain.Entities;
using Dactyloscopy.Domain.Repositories;
using MediatR;

namespace Dactyloscopy.Application.UpdateForensicReviewProcess
{
    public class UpdateForensicReviewProcessCommandHandler(
        IForensicReviewProcessRepository forensicReviewProcessRepository,
        IStatusForensicRepository statusForensicRepository
    ) : IRequestHandler<UpdateForensicReviewProcessCommand>
    {
        private readonly IForensicReviewProcessRepository _forensicReviewProcessRepository = forensicReviewProcessRepository;
        private readonly IStatusForensicRepository _statusForensicRepository = statusForensicRepository;

        public async Task Handle(UpdateForensicReviewProcessCommand request, CancellationToken cancellationToken)
        {
            ForensicReviewProcess? forensicReviewProcess = await _forensicReviewProcessRepository
                .GetForensicReviewProcessByIdAsync(request.ForensicReviewProcessId)
                    ?? throw new KeyNotFoundException($"No se encontró un proceso forense con el id: {request.ForensicReviewProcessId}");

            StatusForensic? statusForensic = await _statusForensicRepository
                .GetStatusByIdAsync(forensicReviewProcess.StatusForensicId)
                    ?? throw new KeyNotFoundException($"No se encontró estado forense con id: {forensicReviewProcess.StatusForensicId}");

            forensicReviewProcess.ValidationProcessId = request.ValidationProcessId;
            forensicReviewProcess.TxGuidForense = request.TxGuidForense;
            forensicReviewProcess.StatusForensicId = request.StatusForensicId;
            forensicReviewProcess.VerificationDate = request.VerificationDate;
            forensicReviewProcess.Approved = request.Approved;
            forensicReviewProcess.Score = request.Score;
            forensicReviewProcess.MainReason = request.MainReason;
            forensicReviewProcess.OptionalReason = request.OptionalReason;
            forensicReviewProcess.Description = request.Description;
            forensicReviewProcess.Observation = request.Observation;
            forensicReviewProcess.UpdatedDate = DateTime.UtcNow.AddHours(-5);

            await _forensicReviewProcessRepository
                .UpdateForensicReviewProcessAsync(forensicReviewProcess);
        }
    }
}
