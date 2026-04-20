namespace Process.Domain.Entities
{


    public class RoleActual
    {
        public long RoleAgreementByUserId { get; set; }
        public long RoleId { get; set; }
        public long UserId { get; set; }
        public string NameRole { get; set; }
        public long AgreementId { get; set; }
        public string NameAgreement { get; set; }

    }
}
