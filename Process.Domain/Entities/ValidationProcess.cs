namespace Process.Domain.Entities
{
    public class ValidationProcess
    {
        public long ValidationProcessId { get; set; }
        public Guid? ValidationProcessGUID { get; set; }
        public Guid? AgreementGUID { get; set; }
        public long? WorkflowId { get; set; }
        public Guid? CitizenGUID { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? InfCandidate { get; set; }
        public string? DocumentTypeId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Email { get; set; }
        public string? CellphoneNumber { get; set; }
        public int? ProcessType { get; set; }
        public bool? Approved { get; set; }
        public long? RejectionCauseId { get; set; }
        public string? Advisor { get; set; }
        public DateTime? DocumentIssuingDate { get; set; }
        public string? DocumentIssuingPlace { get; set; }
        public string? OfficeCode { get; set; }
        public string? OfficeName { get; set; }
        public bool? ExecuteInMobile { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? ForensicState { get; set; }
        public string? ForensicReason { get; set; }
        public string? ForensicOptionalReason { get; set; }
        public string? ForensicObservations { get; set; }
        public bool? Validation { get; set; }
        public long? StatusValidationId { get; set; }
        public int RequestChannel { get; set; }
    }
}
