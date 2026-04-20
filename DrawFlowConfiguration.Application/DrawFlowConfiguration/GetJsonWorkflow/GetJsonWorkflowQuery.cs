using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetJsonWorkflow
{
    public class GetJsonWorkflowQuery : IRequest<object>
    {
        public Guid ProcesoConvenioGuid { get; set; }
        public Guid? AgreementID { get; set; }
        public int? WorkFlowID { get; set; }
    }
}
