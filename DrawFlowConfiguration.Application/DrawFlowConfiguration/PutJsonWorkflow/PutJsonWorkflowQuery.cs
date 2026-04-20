using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PutJsonWorkflow
{
    public class PutJsonWorkflowQuery : IRequest<object>
    {
        public Guid AgreementID { get; set; }
        public int WorkFlowID { get; set; }
        public object drawflow { get; set; } = default!;
    }
}
