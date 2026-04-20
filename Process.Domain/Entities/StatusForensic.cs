namespace Process.Domain.Entities
{
    public class StatusForensic
    {
        public long StatusForensicId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
