namespace Process.Domain.Entities
{
    public class Client
    {
        public long ClientId { get; set; }
        public Guid? ClientToken { get; set; }   
        public string Name { get; set; } = string.Empty;
        public int? DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public int? EconomicSectorId { get; set; }
        public int? CountryId { get; set; }
        public int? LegalRepresentativeId { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? CreatedDate { get; set; }   
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }           
        public bool? IsDeleted { get; set; }      
    }
}
