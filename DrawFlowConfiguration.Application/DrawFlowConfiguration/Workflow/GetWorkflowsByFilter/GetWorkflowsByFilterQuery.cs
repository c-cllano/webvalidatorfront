using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflowsByFilter
{
 
    public class GetWorkflowsByFilterQuery
    : IRequest<IEnumerable<WorkflowsEntry>>
    {
        public Guid? AgreementId { get; set; }
        public int? WorkFlowId { get; set; }
        public string ? Status { get; set; }
    }

}
