namespace Process.Domain.Entities
{
    public class ParametersAgreement
    {
        public long ParameterAgreementId { get; set; }
        public long AgreementId { get; set; }
        public Guid ParameterAgreementGuid { get; set; }
        public string ParameterCode { get; set; } = string.Empty;
        public string ParameterName { get; set; } = string.Empty;
        public string ParameterValue { get; set; } = string.Empty;
        public string? ParameterDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
