namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow
{
    public class WorkflowsNodeEntry
    {
        public int WorkFlowNodeID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Html { get; set; }
        public string? JsonProperties { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }

    }


}
