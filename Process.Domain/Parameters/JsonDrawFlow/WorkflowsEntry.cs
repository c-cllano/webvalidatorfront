
namespace Process.Domain.Parameters.JsonDrawFlow
{
    public class WorkflowsEntry
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public int? CreatorUserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}



