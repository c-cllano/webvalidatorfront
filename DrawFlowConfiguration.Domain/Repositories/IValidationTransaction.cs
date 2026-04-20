using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;

namespace DrawFlowConfiguration.Domain.Repositories
{
    public interface IValidationTransaction
    {
        public Task<bool> SaveValidationTransaction(WorkflowsEntry request);
        public Task<bool> SaveTemplateTransaction(TemplateEntry request);
        public Task<IEnumerable<WorkflowsEntry>> GetAllWorkflows();
        public Task<bool> UpdateWorkflow(SaveWorflowRequest request);
        public Task<IEnumerable<WorkflowsNodeEntry>> GetAllWorkflowNode(int? workFlowNodeID = null, string? name = null);
        public Task<bool> UpdateWorkflowNode(DeleteWorflowNodeRequest request);
        public Task<IEnumerable<WorkflowsEntry>> GetWorkflowsByFilter(int? workFlowId = null, Guid? agreementId = null, string? status =null);
        Task<bool> SaveWorkflowNacionalidadesPermitidas(SaveWorkflowNacionalidadesPermitidasRequest request);
        Task<bool> SaveWorkflowTipoDocumento(SaveWorkflowTipoDocumentoRequest request);
        Task<bool> SaveWorkflowUbicacionesPermitidas(SaveWorkflowUbicacionesPermitidasRequest request);
        Task<IEnumerable<GetExistByNameAndGUIDResponse>> GetExistByNameAndGUID(Guid AgreementID,string Name,int? WorkflowID = null  );
        Task<IEnumerable<DocumentTypeByWorkflow>> GetDocumentosFromItem(int WorkFlowID);
        Task<IEnumerable<NationalityByWorkflow>> GetNationalitiesFromItem(int WorkFlowID);
        Task<IEnumerable<UbicationsByWorkflow>> GetUbicacionesFromItem(int WorkFlowID);
        public Task<bool> DuplicateSQLWorkflow(WorkflowsEntry request);
        public Task<List<NationalityByWorkflow>> GetNationalitiesByWorkflow(int workflowId);
        Task<bool> UpdateNationalityStatus(int workflowId, int countryId, bool status, bool isDeleted);
        Task<bool> InsertNationality(int workflowId, int countryId);
        Task<List<DocumentTypeByWorkflow>> GetDocumentsByWorkflow(int workflowId);
        Task<bool> UpdateDocumentStatus(int workflowId, int documentTypeId, bool status , bool isDeleted);
        Task<bool> InsertDocument(int workflowId, int documentTypeId);  
        Task<List<UbicationsByWorkflow>> GetLocationsByWorkflow(int workflowId);
        Task<bool> UpdateLocationStatus(int workflowId, int countryId, bool status , bool isDeleted);
        Task<bool> InsertLocation(int workflowId, int countryId);
        Task<bool> ArchiveWorkflow(ArchiveWorkflowRequest request);
        Task<bool> DesarchiveWorkflow(DesarchiveWorkflowRequest request);
        Task<bool> PublicarWorkflow(PublicarWorkflowRequest request);
        Task<bool> PauseWorkflow(PauseWorkflowRequest request);
    }
}
