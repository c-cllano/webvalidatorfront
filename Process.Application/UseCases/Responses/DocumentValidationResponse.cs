using Process.Application.ValidationProcesses.SaveDocumentBothSides;

namespace Process.Application.UseCases.Responses
{
    public class DocumentValidationResponse
    {
        public SaveDocumentBothSidesResponse? SaveDocumentBothSidesResponse { get; set; }
        public float Score { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
