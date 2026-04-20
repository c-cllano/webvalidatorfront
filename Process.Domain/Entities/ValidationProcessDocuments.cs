namespace Process.Domain.Entities
{
    public class ValidationProcessDocuments
    {
        public long ValidationProcessDocumentsId { get; set; }
        public long ValidationProcessId { get; set; }
        public string? Trazability { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
