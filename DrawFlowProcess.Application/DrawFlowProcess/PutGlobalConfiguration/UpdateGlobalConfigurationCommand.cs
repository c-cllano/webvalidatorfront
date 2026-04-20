using MediatR;
using System.Text.Json;

namespace DrawFlowProcess.Application.DrawFlowProcess.PutGlobalConfiguration
{
    public class UpdateGlobalConfigurationCommand : IRequest<bool>
    {
        public JsonDocument? Document { get; set; }
        public Guid AgreementId { get; set; }
        public int WorkFlowId { get; set; }
    }
}
