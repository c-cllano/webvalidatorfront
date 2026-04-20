using MediatR;
using Microsoft.AspNetCore.Http;
using Process.Application.ValidationProcesses.ValidateBiometric;

// Inicio código generado por GitHub Copilot
namespace Process.Application.ValidationProcesses.ValidateBiometricV3
{
    public record ValidateBiometricV3Command(
        IFormFile Biometric,
        IFormFile BiometricGesture,
        Guid CitizenGUID,
        long ValidationProcessId,
        string SubType,
        int ServiceId,
        string? CodeParameter
    ) : IRequest<ValidateBiometricResponse>;
}
// Fin código generado por GitHub Copilot
