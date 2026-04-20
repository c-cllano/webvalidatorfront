namespace Process.Application.AgreementByUser.GetByUserId
{
    public class GetByUserIdQueryResponse
    {
        public string AgreementName { get; set; } = default!;
        public Guid AgreementGuid { get; set; }
        public bool Active { get; set; }
        public long AgreementByUserId { get; set; }
        public int AgreementId { get; set; }

        public string? ClientToken { get; set; }
    }
}
