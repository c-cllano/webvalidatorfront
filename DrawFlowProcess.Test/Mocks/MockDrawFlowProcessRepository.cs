using DrawFlowProcess.Domain.Repositories;
using DrawFlowProcess.Domain.Domain;
using System.Text.Json;

namespace DrawFlowProcess.Test.Mocks
{
    // Corrige el nombre de la interfaz si es necesario
    public class MockDrawFlowProcessRepository : IDrwaFlowProcessRepository
    {
        public ExportJson GetJsonConvert(JsonDocument jsonDocument)
        {
            return new ExportJson
            {
                AgreementID = Guid.NewGuid(),
                WorkflowID = 1,
                Nodos = new List<Nodo>()
            };
        }

        public async Task<bool> SaveJsonConvert(ExportJson json)
        {
            await Task.Delay(10);
            return true;
        }

        public async Task<ExportJson> GetJsonByIds(Guid AgreementId, int workFlowId)
        {
            await Task.Delay(10);
            return new ExportJson
            {
                AgreementID = AgreementId,
                WorkflowID = workFlowId,
                Nodos = new List<Nodo>()
            };
        }

        public async Task<ProcessFlow> GetProcessFlow(Guid AgreementId, int workFlowId, string typeCurrent, JsonDocument conditional = null!, int typeProcess = 0, bool back = false)
        {
            await Task.Delay(10);
            return new ProcessFlow
            {
                Conditional = false,
                CountPages = 1,
                TypeFrom = typeCurrent,
                TypeFront = new List<string> { "Front" },
                TypeBack = new List<string> { "Back" },
                DataConfiguration = null
            };
        }

        public async Task<JsonPages> GetJsonPages(Guid AgreementId, int workFlowId)
        {
            await Task.Delay(10);
            return new JsonPages
            {
                CountPages = 1,
                Pages = new Dictionary<string, object>()
            };
        }
    }
}
