using System.Text.Json;
using DrawFlowProcess.Domain.Domain;

namespace DrawFlowProcess.Domain.Repositories
{
    public interface IDrwaFlowProcessRepository
    {
        public ExportJson GetJsonConvert(JsonDocument jsonDocument);
        public Task<bool> SaveJsonConvert(ExportJson exportJson);
        public Task<ExportJson> GetJsonByIds(Guid agreementId, int workFlowId);
        public Task<ProcessFlow> GetProcessFlow(Guid agreementId, int workFlowId, string typeCurrent, JsonDocument conditional = null!, int typeProcess = 0, bool back = false);
        public Task<JsonPages> GetJsonPages(Guid agreementId, int workFlowId);
    }
}
