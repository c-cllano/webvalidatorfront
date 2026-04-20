namespace Process.Domain.Entities
{
    public class ValidationProcessScores
    {
        public long ValidationProcessScoresId { get; set; }
        public long ValidationProcessId { get; set; }
        public string Scores { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
