using MongoDB.Bson;

namespace DrawFlowProcess.Domain.Domain
{
    public class Nodo
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required Connection Connections { get; set; }
        public required Dictionary<string, object> Data { get; set; }
    }
}
