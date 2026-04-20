using System.Text.Json;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PostJsonWorkflows
{
    public class SaveJsonWorkflowQuery : IRequest<object>
    {
        public object drawflow { get; set; } = default!;
        public int WorkFlowID { get; set; }
        public Guid? AgreementID { get; set; }
    }


}
