using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.DeleteJsonWorkflow
{


    public class DeleteJsonWorkflowCommand : IRequest<object>
    {
        public Guid AgreementID { get; set; }
        public int WorkFlowID { get; set; }
    }
}
