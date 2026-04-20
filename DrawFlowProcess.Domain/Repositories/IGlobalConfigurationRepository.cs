using System.Text.Json;
using DrawFlowProcess.Domain.Domain;

namespace DrawFlowProcess.Domain.Repositories
{
    public interface IGlobalConfigurationRepository
    {
        Task<bool> SaveGlobalConfigurationAsync(JsonDocument globalConfiguration);
        Task<List<GlobalConfiguration>> GetGlobalConfigurationsAsync(Guid agreementId, int workFlowId, DateTime createDateTask, string section);
        Task<bool> UpdateGlobalConfigurationAsync(Guid agreementId, int workFlowId, JsonDocument globalConfiguration);
        Task<JsonDocument> GetGlobalConfigurationByFlow(Guid agreementId, int workFlowId);
    }
}
