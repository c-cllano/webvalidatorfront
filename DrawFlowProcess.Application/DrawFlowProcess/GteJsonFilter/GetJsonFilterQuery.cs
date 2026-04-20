using DrawFlowProcess.Domain.Domain;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GteJsonFilter
{
    public class GetJsonFilterQuery : IRequest<ExportJson>
    {
        public Guid AgreementId { get; set; }
        public int WorkflowId {  get; set; }
    }
}
