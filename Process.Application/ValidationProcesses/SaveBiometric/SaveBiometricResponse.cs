using Process.Application.UseCases.Responses;

namespace Process.Application.ValidationProcesses.SaveBiometric
{
    public class SaveBiometricResponse
    {
        public bool IsHomologation { get; set; }
        public bool IsSuccessful { get; set; }
        public IEnumerable<ErrorTransactionResponse> TransactionError { get; set; } = default!;
    }
}
