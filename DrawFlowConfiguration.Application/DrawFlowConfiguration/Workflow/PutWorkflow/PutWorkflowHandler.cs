using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;
using System.Runtime.CompilerServices;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.PutWorkflow
{
    public class PutWorkflowHandler : IRequestHandler<UpdateWorkflowCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public PutWorkflowHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
        {
            var data = request.Request;

            // 1. Actualizar los datos básicos
            var basicUpdateOk = await _transaction.UpdateWorkflow(data);
            if (!basicUpdateOk) return false;

            // 2. Sincronizar Nacionalidades si no es null
            if (data.NationalityByWorkflow != null)
            {
                await SynchronizeNationalities(data.WorkFlowID, data.NationalityByWorkflow);
            }

            // 3. Sincronizar Tipos de Documentos si no es null
            if (data.DocumentTypeByWorkflow != null)
            {
                await SynchronizeDocumentTypes(data.WorkFlowID, data.DocumentTypeByWorkflow);
            }

            // 4. Sincronizar Ubicaciones si no es null
            if (data.UbicationsByWorkflow != null)
            {
                await SynchronizeLocations(data.WorkFlowID, data.UbicationsByWorkflow);
            }

            return true;
        }


        private async Task SynchronizeNationalities(int workflowId, List<NationalityByWorkflow>? incomingList)
        {
            var currentDbList = await _transaction.GetNationalitiesByWorkflow(workflowId);
            var incomingIds = incomingList?.Select(x => x.CountryId).ToList() ?? new List<int>();

            // A. ELIMINADO LÓGICO: Si está activo y no viene en el request -> Borrar
            foreach (var item in currentDbList.Where(db => db.Active && !incomingIds.Contains(db.CountryId)))
            {
                await _transaction.UpdateNationalityStatus(workflowId, item.CountryId, false, true);
            }

            // B. MANTENER / REACTIVAR / INSERTAR
            foreach (var id in incomingIds)
            {
                var existing = currentDbList.FirstOrDefault(db => db.CountryId == id);

                if (existing != null)
                {
                    // REACTIVACIÓN: Si existe en la DB (aunque esté borrado o inactivo) 
                    // y el usuario lo envió en el request, lo ponemos en Active=1 e IsDeleted=0
                    if (!existing.Active || existing.IsDeleted)
                    {
                        await _transaction.UpdateNationalityStatus(workflowId, id, true, false);
                    }
                }
                else
                {
                    // C. INSERTAR: Solo si el ID realmente nunca ha existido para este workflow
                    await _transaction.InsertNationality(workflowId, id);
                }
            }
        }
        private async Task SynchronizeDocumentTypes(int workflowId, List<DocumentTypeByWorkflow>? incomingList)
        {
            var currentDbList = await _transaction.GetDocumentsByWorkflow(workflowId);
            var incomingIds = incomingList!.Select(x => x.DocumentTypeId).ToList();

            // Inactivar
            foreach (var item in currentDbList.Where(db => db.Active && !incomingIds.Contains(db.DocumentTypeId)))
            {
                await _transaction.UpdateDocumentStatus(workflowId, item.DocumentTypeId, false, true);
            }

            // Reactivar o Insertar
            foreach (var id in incomingIds)
            {
                var existing = currentDbList.FirstOrDefault(db => db.DocumentTypeId == id);
                if (existing != null)
                {
                    if (!existing.Active) await _transaction.UpdateDocumentStatus(workflowId, id, true, false);
                }
                else
                {
                    await _transaction.InsertDocument(workflowId, id);
                }
            }
        }

        private async Task SynchronizeLocations(int workflowId, List<UbicationsByWorkflow>? incomingList)
        {
            var currentDbList = await _transaction.GetLocationsByWorkflow(workflowId);
            var incomingIds = incomingList!.Select(x => x.CountryId).ToList();

            // Inactivar
            foreach (var item in currentDbList.Where(db => db.Active && !incomingIds.Contains(db.CountryId)))
            {
                await _transaction.UpdateLocationStatus(workflowId, item.CountryId, false, true);
            }

            // Reactivar o Insertar
            foreach (var id in incomingIds)
            {
                var existing = currentDbList.FirstOrDefault(db => db.CountryId == id);
                if (existing != null)
                {
                    if (!existing.Active) await _transaction.UpdateLocationStatus(workflowId, id, true, false);
                }
                else
                {
                    await _transaction.InsertLocation(workflowId, id);
                }
            }
        }
    }


}
