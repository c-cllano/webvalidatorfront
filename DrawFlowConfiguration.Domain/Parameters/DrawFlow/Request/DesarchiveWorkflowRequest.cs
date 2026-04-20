namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request
{
    public class DesarchiveWorkflowRequest
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
