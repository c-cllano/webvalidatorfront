using MediatR;

namespace Process.Application.ValidationProcesses.UpdateStatusValidationProcess
{
    public record UpdateStatusValidationProcessCommand(
        long ValidationProcessId,
        int StatusCode,
        bool Approved,
        bool Active
    ) : IRequest<UpdateStatusValidationProcessResponse>;
}
