using MediatR;

// Inicio código generado por GitHub Copilot
namespace Process.Application.ValidationProcesses.CancelProcess
{
    public record CancelProcessCommand(
        Guid ValidationProcessGuid,
        string Adviser,
        string Reason,
        int ReasonId
    ) : IRequest<CancelProcessResponse>;
}
// Fin código generado por GitHub Copilot
