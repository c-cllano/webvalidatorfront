using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.ArchiveWorkflow
{
    public class ArchiveWorkflowQuery : IRequest<object>
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public int? CreatorUserID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
