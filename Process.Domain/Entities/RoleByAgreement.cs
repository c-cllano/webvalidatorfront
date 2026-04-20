namespace Process.Domain.Entities
{
    /// <summary>
    /// Entidad que representa la relación entre un rol y un convenio/agreement
    /// </summary>
    public class RoleByAgreement
    {
        public long RoleByAgreementId { get; set; }
        public long RoleId { get; set; }
        public int AgreementId { get; set; }
        public long CreatorUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        
        // Navigation properties
        public string? RoleName { get; set; }
        public string? AgreementName { get; set; }
    }
}
