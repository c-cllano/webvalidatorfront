using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DrawFlowProcess.Domain.Domain
{
    public class ExportJson
    {
        [BsonRepresentation(BsonType.String)]
        public Guid AgreementID { get; set; }
        public int WorkflowID { get; set; }
        public List<Nodo>? Nodos { get; set; }
    }
}
