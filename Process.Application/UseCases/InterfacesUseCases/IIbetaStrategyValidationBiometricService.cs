using Process.Application.ValidationProcesses.ValidateBiometric;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IIbetaStrategyValidationBiometricService
    {
        Task<ValidateBiometricResponse> ExecuteAsync(ValidateBiometricCommand request, Guid agreementGuid, string urlBiometric, string urlBiometricGesture, string imageBiometricCitizen, string mimeType);
    }
}
