using MediatR;

namespace Process.Application.ValidationProcesses.ConsultValidation
{
    public record ConsultValidationCommand(
        Guid ValidationProcessGUID
    ) : IRequest<ConsultValidationResponse>;
}
