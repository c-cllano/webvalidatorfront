namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request
{
    public class SaveWorflowRequest
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public int? CreatorUserID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int? WorkflowStatesId { get; set; }


        /** listados para edicion **/
        public List<NationalityByWorkflow> ? NationalityByWorkflow { get; set; }
        public List<DocumentTypeByWorkflow> ? DocumentTypeByWorkflow { get; set; }
        public List <UbicationsByWorkflow> ? UbicationsByWorkflow { get; set; }
    }
}
