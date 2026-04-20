using System.Text.Json;
using System.Text.Json.Nodes;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetProcessFlow
{
    public class GetProcessFlowResponse
    {
        public bool Conditional { get; set; }
        public int CountPages { get; set; }
        public string? CurrentStep { get; set; }
        public List<string>? NextStep { get; set; }
        public List<string>? BackStep { get; set; }
        public JsonObject? DataConfiguration { get; set; }
    }
}
