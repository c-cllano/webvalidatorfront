namespace Process.Domain.Entities
{
    public class RoleAgreementByUser
    {
        public long RoleAgreementByUserId { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public int AgreementId { get; set; }
        public long CreatorUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
