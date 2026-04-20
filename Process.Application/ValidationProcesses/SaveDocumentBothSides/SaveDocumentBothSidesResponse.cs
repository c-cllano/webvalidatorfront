using Process.Application.UseCases.Responses;

namespace Process.Application.ValidationProcesses.SaveDocumentBothSides
{
    public class SaveDocumentBothSidesResponse
    {
        public Guid? FrontalGUID { get; set; }
        public Guid? ReverseGUID { get; set; }
        public bool? FrontalSuccessful { get; set; }
        public bool? ReverseSuccessful { get; set; }
        public string? FrontalMessage { get; set; }
        public string? ReverseMessage { get; set; }
        public string? DocumentTypeDescription { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? Sex { get; set; }
        public string? Rh { get; set; }
        public string? PlaceBirth { get; set; }
        public DateTime? DateBirth { get; set; }
        public bool? IsHomologation { get; set; }
        public bool? IsSuccessful { get; set; }
        public string? PlaceOfIssue { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public IEnumerable<ErrorTransactionResponse> TransactionError { get; set; } = default!;
    }
}
