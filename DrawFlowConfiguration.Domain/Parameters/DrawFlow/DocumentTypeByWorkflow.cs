namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow
{
    public class DocumentTypeByWorkflow
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public int WorkflowsTipoDocumentosPermitidosId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? NameDocType { get; set; }
        public int Length { get; set; }
        public string RegularExpression { get; set; }
        public bool Active { get; set; }
        public bool? IsDeleted { get; set; }


        /** **/
        public string? name { get; set; }
    }
}
