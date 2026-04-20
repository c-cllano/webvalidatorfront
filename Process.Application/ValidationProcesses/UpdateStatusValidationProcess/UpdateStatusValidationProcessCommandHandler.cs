using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.ValidationProcesses.UpdateStatusValidationProcess
{
    public class UpdateStatusValidationProcessCommandHandler(
        IValidationProcessRepository validationProcessRepository,
        IStatusValidationRepository statusValidationRepository
    ) : IRequestHandler<UpdateStatusValidationProcessCommand, UpdateStatusValidationProcessResponse>
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IStatusValidationRepository _statusValidationRepository = statusValidationRepository;

        public async Task<UpdateStatusValidationProcessResponse> Handle(UpdateStatusValidationProcessCommand request, CancellationToken cancellationToken)
        {
            ValidationProcess? validationProcess = await _validationProcessRepository
                .GetValidationProcessById(request.ValidationProcessId)
                    ?? throw new KeyNotFoundException("El proceso no fue encontrado.");

            StatusValidation? statusValidation = await _statusValidationRepository
                .GetStatusValidationByStatusCodeAsync(request.StatusCode)
                    ?? throw new KeyNotFoundException($"No existe un estado de validación con StatusCode: {request.StatusCode}");

            validationProcess.StatusValidationId = statusValidation.StatusValidationId;
            validationProcess.Approved = request.Approved;
            validationProcess.Active = request.Active;
            validationProcess.UpdatedDate = DateTime.UtcNow.AddHours(-5);

            await _validationProcessRepository.UpdateValidationProcessAsync(validationProcess);

            return new()
            {
                ValidationProcessId = validationProcess.ValidationProcessId,
                StatusCode = request.StatusCode,
                StatusValidationId = statusValidation.StatusValidationId,
                Approved = validationProcess.Approved.Value,
                Active = validationProcess.Active.Value
            };
        }
    }
}
