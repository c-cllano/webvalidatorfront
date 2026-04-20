using MediatR;

namespace Process.Application.ValidationProcesses.ValidateBiometric
{
    public record ValidateBiometricCommand(
        Guid CitizenGUID,
        long ValidationProcessId,
        string Format,
        string SubType,
        int ServiceId,
        string Biometric,
        string BiometricGesture,
        string FormatGesture,
        string? AntiSpoofing
    ) : IRequest<ValidateBiometricResponse>;
}
