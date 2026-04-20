namespace UIConfiguration.Domain.Repositories
{
    public interface IUIConfigurationRepository
    {
        public Task<object> GetUIConfiguration(string clientId, int workflowID);
        public Task<object> SaveUIConfiguration(object jsonConfiguration);
        public Task<object> UpdateIUConfiguration(object jsonConfiguration, string clientId, int workflowID);
        public Task<List<string>> GetPagesWithDisplayTrue(string clientId, int workflowID);
        public Task<object?> GetGlobalConfiguration(string clientId, int workflowID);
        public Task<object?> GetPageIfVisible(string clientId, string pageName, int workflowID);
        public Task<object?> UpdateDynamicFieldsAsync(string clientId, int workflowID, Dictionary<string, object> updates);
        public Task<object?> AddDynamicFieldsAsync(string clientId, int workflowID, Dictionary<string, object> fieldsToAdd);
        public Task<object?> AddIfNotExistsAsync(string clientId, int workflowID, Dictionary<string, object> fieldsToAdd);
        public Task<object> AddIfNotExistsToAllAsync(Dictionary<string, object> fieldsToAdd);
    }
}
