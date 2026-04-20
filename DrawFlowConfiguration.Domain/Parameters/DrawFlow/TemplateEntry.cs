namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow
{
    public class TemplateEntry
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public int? CreatorUserID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(-5);
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
