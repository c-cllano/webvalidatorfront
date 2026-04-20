using MediatR;

namespace Process.Application.ValidationProcesses.SaveBiometric
{
    public record SaveBiometricCommand(
        Guid CitizenGUID,
        long ValidationProcessId,
        int ServiceId,
        string SubType,
        string Value,
        string Format,
        string AditionalData,
        string User,
        bool Update,
        string CodeParameter,
        string BiometricGesture,
        string FormatGesture,
        string? AntiSpoofing
    ) : IRequest<SaveBiometricResponse>;
}
