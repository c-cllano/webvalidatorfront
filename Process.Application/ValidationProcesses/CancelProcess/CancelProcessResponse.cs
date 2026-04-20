// Inicio código generado por GitHub Copilot
using Process.Application.UseCases.Responses;

namespace Process.Application.ValidationProcesses.CancelProcess
{
    public class CancelProcessResponse
    {
        public bool IsHomologation { get; set; }
        public bool IsSuccessful { get; set; }
        public IEnumerable<ErrorTransactionResponse> TransactionError { get; set; } = default!;
    }
}
// Fin código generado por GitHub Copilot
