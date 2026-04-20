using Process.Application.UseCases.Responses;

namespace Process.Application.ValidationProcesses.ValidateBiometric
{
    public class ValidateBiometricResponse
    {
        public bool IsValid { get; set; }
        public object Result { get; set; } = default!;
        public decimal Score { get; set; }
        public bool IsHomologation { get; set; }
        public bool IsSuccessful { get; set; }
        public IEnumerable<ErrorTransactionResponse> TransactionError { get; set; } = default!;
    }
}
