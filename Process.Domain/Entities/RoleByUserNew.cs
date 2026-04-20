namespace Process.Domain.Entities
{
    public class RoleByUserNew
    {

        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string NameRole { get; set; } 
        public string NameAgreement { get; set; }
        public long AgreementId { get; set; }

    }
}
