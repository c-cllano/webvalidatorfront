using Process.Application.ValidationProcesses.SaveBiometric;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IIbetaStrategySaveBiometricService
    {
        Task<SaveBiometricResponse> ExecuteAsync(SaveBiometricCommand request, long citizenId, string urlBiometric, string urlBiometricGesture);
    }
}
