namespace Process.Domain.Entities
{
    public class DocumentTypeCapture
    {
        public int DocumentTypeCaptureId { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? Sides { get; set; }
        public bool InstantFeedback { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}