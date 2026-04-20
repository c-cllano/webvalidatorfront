namespace Process.Domain.Entities
{
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Length { get; set; }
        public string? RegularExpression { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
