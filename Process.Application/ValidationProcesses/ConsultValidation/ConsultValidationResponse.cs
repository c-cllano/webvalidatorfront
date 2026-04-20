namespace Process.Application.ValidationProcesses.ConsultValidation
{
    public class ConsultValidationResponse
    {
        public Guid AgreementGUID { get; set; }
        public Guid ValidationProcessGUID { get; set; }
        public Guid CitizenGUID { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public string? PlaceOfIssue { get; set; }
        public int? ProcessStatus { get; set; }
        public bool? Approved { get; set; }
        public bool? IsCompleted { get; set; }
        public int? ForensicState { get; set; }
        public string? ForensicReason { get; set; }
        public string? ForensicOptionalReason { get; set; }
        public string? ForensicObservations { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? RejectionCauseId { get; set; }
        public string? RejectionCauseDescription { get; set; }
    }
}
