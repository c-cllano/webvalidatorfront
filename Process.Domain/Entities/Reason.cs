namespace Process.Domain.Entities
{
    public class Reason
    {
        public long ReasonID { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Approved { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
    }
}
