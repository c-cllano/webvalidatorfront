using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflow
{
    public class GetWorkflowQuery : IRequest<object>
    {
        public int ProcesoConvenioGuid { get; set; }
    }

 
}
