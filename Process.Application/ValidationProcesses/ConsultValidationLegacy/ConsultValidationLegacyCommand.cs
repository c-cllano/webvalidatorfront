using MediatR;

namespace Process.Application.ValidationProcesses.ConsultValidationLegacy
{
    public record ConsultValidationLegacyCommand(
        Guid ValidationProcessGUID
    ) : IRequest<string>;
}
