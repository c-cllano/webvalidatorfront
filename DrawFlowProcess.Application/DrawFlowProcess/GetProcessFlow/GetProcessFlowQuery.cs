using System.Text.Json;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetProcessFlow
{
    public class GetProcessFlowQuery : IRequest<GetProcessFlowResponse>
    {
        public string? NameType { get; set; }
        public Guid AgreeentId { get; set; }
        public int WorkFlowId { get; set; }
        public JsonDocument Conditional { get; set; } = null!;
        public int TypeProcess { get; set; } = 0;
        public bool Back { get; set; } = false;
    }
}
