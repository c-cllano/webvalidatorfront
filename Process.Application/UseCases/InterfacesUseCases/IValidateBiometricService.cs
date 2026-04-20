using Process.Application.ValidationProcesses.ValidateBiometric;
using Process.Domain.Entities;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IValidateBiometricService
    {
        Task<ValidateBiometricResponse> ValidateBiometricAsync(ValidateBiometricCommand request, ValidationProcess validationProcess);
    }
}
