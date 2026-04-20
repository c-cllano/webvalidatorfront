namespace Process.Domain.Entities
{
    public class ForensicReviewProcess
    {
        public long ForensicReviewProcessId { get; set; }
        public long ValidationProcessId { get; set; }
        public Guid? TxGuidForense { get; set; }
        public long StatusForensicId { get; set; }
        public DateTime? VerificationDate { get; set; }
        public bool? Approved { get; set; }
        public decimal? Score { get; set; }
        public string? MainReason { get; set; }
        public string? OptionalReason { get; set; }
        public string? Description { get; set; }
        public string? Observation { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
