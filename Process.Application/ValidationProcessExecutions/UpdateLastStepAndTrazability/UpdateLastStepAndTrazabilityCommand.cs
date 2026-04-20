using MediatR;
using Process.Domain.Entities;

namespace Process.Application.ValidationProcessExecutions.UpdateLastStepAndTrazability
{
    public record UpdateLastStepAndTrazabilityCommand(
        long ValidationProcessExecutionId,
        string LastStep,
        string Trazability
    ) : IRequest<UpdateLastStepAndTrazabilityResponse>;
}
