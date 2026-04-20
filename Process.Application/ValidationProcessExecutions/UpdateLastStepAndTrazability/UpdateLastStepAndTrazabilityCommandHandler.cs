using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.ValidationProcessExecutions.UpdateLastStepAndTrazability
{
    public class UpdateLastStepAndTrazabilityCommandHandler(
        IValidationProcessExecutionRepository validationProcessExecutionRepository
    ) : IRequestHandler<UpdateLastStepAndTrazabilityCommand, UpdateLastStepAndTrazabilityResponse>
    {
        private readonly IValidationProcessExecutionRepository _validationProcessExecutionRepository = validationProcessExecutionRepository ?? throw new ArgumentNullException(nameof(validationProcessExecutionRepository));

        public async Task<UpdateLastStepAndTrazabilityResponse> Handle(UpdateLastStepAndTrazabilityCommand request, CancellationToken cancellationToken)
        {
            ValidationProcessExecution? validationProcessExecution = await _validationProcessExecutionRepository
                .GetValidationProcessExecutionById(request.ValidationProcessExecutionId)
                    ?? throw new KeyNotFoundException("El proceso de ejecución no fue encontrado.");

            validationProcessExecution.LastStep = request.LastStep;
            validationProcessExecution.Trazability = request.Trazability;
            validationProcessExecution.UpdatedDate = DateTime.UtcNow.AddHours(-5);

            await _validationProcessExecutionRepository.UpdateValidationProcessExecutionAsync(validationProcessExecution);

            return new()
            {
                ValidationProcessExecutionId = validationProcessExecution.ValidationProcessExecutionId,
                ValidationProcessId = validationProcessExecution.ValidationProcessId,
                LastStep = validationProcessExecution.LastStep,
                Trazability = validationProcessExecution.Trazability
            };
        }
    }
}
