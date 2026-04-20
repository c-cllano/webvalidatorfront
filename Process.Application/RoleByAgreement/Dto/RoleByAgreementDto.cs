namespace Process.Application.RoleByAgreement.Dto
{
    public class RoleByAgreementDto
    {
        public long RoleByAgreementId { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int AgreementId { get; set; }
        public string AgreementName { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateRoleByAgreementRequest
    {
        public long RoleId { get; set; }
        public int AgreementId { get; set; }
        public long CreatorUserId { get; set; }
    }

    public class UpdateRoleByAgreementRequest
    {
        public bool Active { get; set; }
    }

    public class UpdateRoleAgreementsRequest
    {
        public List<int> AgreementIds { get; set; } = [];
        public long UpdaterUserId { get; set; }
    }
}
