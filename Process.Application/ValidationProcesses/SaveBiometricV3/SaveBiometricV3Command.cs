using MediatR;
using Microsoft.AspNetCore.Http;
using Process.Application.ValidationProcesses.SaveBiometric;

// Inicio código generado por GitHub Copilot
namespace Process.Application.ValidationProcesses.SaveBiometricV3
{
    public record SaveBiometricV3Command(
        IFormFile Value,
        IFormFile BiometricGesture,
        long ValidationProcessId,
        Guid CitizenGUID,
        int ServiceId,
        string SubType,
        string? AditionalData,
        string User,
        bool Update,
        string? CodeParameter
    ) : IRequest<SaveBiometricResponse>;
}
// Fin código generado por GitHub Copilot
