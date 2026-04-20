namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow
{
    public class WorkflowsEntry
    {
        public int? RowNum { get; set; }
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public int? CreatorUserID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public bool Active { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? WorkflowStatesId { get; set; }

        /*** Estados del flujo **/
        public string? State { get; set; }
        public string? BackgroundColor { get; set; }
        public string? FontColor { get; set; }

        /** Listado de items asociados al flujo **/
        public List<NationalityByWorkflow>? NationalityByWorkFlow { get; set; }
        public List<UbicationsByWorkflow>? UbicationsByWorkFlow { get; set; }
        public List<DocumentTypeByWorkflow>? DocumentTypeByWorkFlow { get; set; }

        public string? Nationalities { get; set; }
        public string? Ubications { get; set; }
    }

}
