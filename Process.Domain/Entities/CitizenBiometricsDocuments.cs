namespace Process.Domain.Entities
{
    public class CitizenBiometricsDocuments
    {
        public long CitizenBiometricsDocumentsId { get; set; }
        public Guid CitizenBiometricsDocumentsGuid { get; set; }
        public long CitizenId { get; set; }
        public string UrlFile { get; set; } = string.Empty;
        public int ServiceType { get; set; }
        public int? ServiceSubType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
