using System.Text.Json;
using System.Text.Json.Nodes;

namespace DrawFlowProcess.Domain.Domain
{
    public class ProcessFlow
    {
        public bool Conditional { get; set; }
        public int CountPages { get; set; }
        public string? TypeFrom { get; set; }
        public List<string>? TypeFront { get; set; }
        public List<string>? TypeBack { get; set; }
        public JsonObject? DataConfiguration { get; set; }        
    }
}
