namespace DrawFlowConfiguration.Domain.Repositories
{
    public interface IJsonWorkflowRepository
    {

        public Task<object> GetUIConfiguration(Guid? agreementId = null, int? workFlowId = null);
        public Task<object> SaveUIConfiguration(object jsonConfiguration);
        public Task<bool> UpdateUIConfiguration(Guid agreementId, int workFlowId, object jsonConfiguration);

        public Task<bool> DeleteWorkflow(Guid agreementId, int workFlowId);
    }
}
