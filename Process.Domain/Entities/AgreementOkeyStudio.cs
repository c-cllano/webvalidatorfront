namespace Process.Domain.Entities
{
    public class AgreementOkeyStudio
    {
        public long AgreementId { get; set; }
        public Guid AgreementGUID { get; set; }
        public long? ClientId { get; set; }
        public int? Status { get; set; }
        public string? Name { get; set; }
        public string? ATDPToken { get; set; }
        public string? UserReconoserId { get; set; }
        public string? PasswordReconoserId { get; set; }
        public string? PlatformConnection { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? ChangeUrl { get; set; }
        public string? BaseUrlReconoser1 { get; set; }
        public string? BaseUrlReconoser2 { get; set; }
    }
}
