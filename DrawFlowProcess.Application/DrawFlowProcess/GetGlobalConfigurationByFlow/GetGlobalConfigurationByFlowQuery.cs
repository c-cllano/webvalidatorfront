using MediatR;
using System.Text.Json;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetGlobalConfigurationByFlow
{
    public class GetGlobalConfigurationByFlowQuery : IRequest<JsonDocument>
    {
        public Guid AgreementId { get; set; }
        public int WorkFlowId { get; set; }
    }
}
