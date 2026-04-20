namespace DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Response
{
    public class ScreenFlowResponse
    {

        public int ScreenFlowID { get; set; }               
        public Guid AgreementID { get; set; }
        public int? SelectedIdWorkflow { get; set; }
        public int? ContScreenFlow { get; set; }
        public int? OperationScreenFlowID { get; set; }       
        public bool StateScreenFlow { get; set; } = true;  
        public int? CreatorUserID { get; set; }             
        public DateTime CreatedDate { get; set; }           
        public DateTime? UpdatedDate { get; set; }          
        public bool Active { get; set; } = true;            
        public bool IsDeleted { get; set; } = false;      
    }
}
