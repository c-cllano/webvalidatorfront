using Process.Application.ValidationProcesses.SaveBiometric;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface ISaveBiometricService
    {
        Task<SaveBiometricResponse> SaveBiometricAsync(SaveBiometricCommand request);
    }
}
