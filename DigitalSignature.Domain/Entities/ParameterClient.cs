namespace DigitalSignature.Domain.Entities
{
    public class ParameterClient
    {
        public long ParameterClientId { get; set; }
        public Guid ParameterClientGuid { get; set; }
        public long ClientId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public string ParameterValue { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
