namespace Process.Domain.Entities
{
    public class AgreementByUser
    {
        public long AgreementByUserId { get; set; }
        public int UserId { get; set; }
        public int AgreementId { get; set; }
        public long? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }

        public string AgreementName { get; set; } = default!;
        public Guid AgreementGUID { get; set; }

        public string? clientToken { get; set; }

    }

}
