
namespace Process.Domain.Parameters.JsonDrawFlow
{
    public class TemplateEntry
    {
        public int TemplateID { get; set; }
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public int? CreatorUserID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(-5);
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; } = true;
        public bool IsDeleted { get; set; } = false;


    }
}
