namespace Process.Domain.Entities
{
    public class StatusValidation
    {
        public long StatusValidationId { get; set; }
        public int StatusCode { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
