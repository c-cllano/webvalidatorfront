using Process.Application.ValidationProcesses.ValidateBiometric;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IValidateBiometricsByProcess
    {
        Task<List<ValidateBiometricResponse>> ValidateBiometricByProcessAsync(int validationProcessId, int citizenId, int processType);
    }
}
