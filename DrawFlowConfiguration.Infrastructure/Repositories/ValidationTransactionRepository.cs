using Dapper;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentosFromItem;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetExistByNameAndGUID;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetNationalitiesFromItem;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetUbicacionesFromItem;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.PutWorkflow;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using DrawFlowConfiguration.Domain.Parameters.Transaction;
using DrawFlowConfiguration.Domain.Repositories;
using DrawFlowConfiguration.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Net.NetworkInformation;
using System.Text;

namespace DrawFlowConfiguration.Infrastructure.Repositories
{
    public class ValidationTransactionRepository(SQLServerConnectionFactory connectionFactory) : IValidationTransaction
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();

        public async Task<bool> SaveValidationTransaction(WorkflowsEntry request)
        {

            string sqlCommand = @"INSERT INTO Workflows 
                                (AgreementID, CreatorUserID, Name, Description, Version, CreatedDate, UpdatedDate, Active, IsDeleted, Workflowstatesid) 
                                VALUES 
                                 (@AgreementID, @CreatorUserID, @Name, @Description, @Version, @CreatedDate, @UpdatedDate, @Active, @IsDeleted, @Workflowstatesid)";


            var result = await _connection.ExecuteAsync(sqlCommand, request);

            return result > 0;
        }

        public async Task<bool> SaveTemplateTransaction(TemplateEntry request)
        {
                            
            var sqlCommand = @" INSERT INTO Template 
                (WorkFlowID, AgreementID, Name, Description, Version, CreatorUserID, CreatedDate, UpdatedDate, Active, IsDeleted)
            VALUES 
                (@WorkFlowID, @AgreementID, @Name, @Description, @Version, @CreatorUserID, @CreatedDate, @UpdatedDate, @Active, @IsDeleted)";


            var result = await _connection.ExecuteAsync(sqlCommand, request);

            return result > 0;
        }

        public async Task<IEnumerable<WorkflowsEntry>> GetAllWorkflows()
        {
            string sqlQuery = @"SELECT 
                            WorkFlowID,
                            AgreementID,
                            CreatorUserID,
                            Name,
                            Description,
                            Version,
                            CreatedDate,
                            UpdatedDate,
                            Active,
                            IsDeleted,
                            Workflowstatesid
                        FROM Workflows
                        WHERE IsDeleted = 0"; // solo los no eliminados

            var result = await _connection.QueryAsync<WorkflowsEntry>(sqlQuery);
            return result;
        }

        public async Task<IEnumerable<WorkflowsEntry>> GetWorkflowsByFilter(
            int? workFlowId = null,
            Guid? agreementId = null,
            string? status = null)
        {
            var sql = new StringBuilder(@"
        -- Query 1: Workflows (guardado en temporal)
        SELECT 
            ISNULL(CAST(ROW_NUMBER() OVER (ORDER BY w.WorkFlowID) AS INT), 0) AS RowNum,
            w.WorkFlowID,
            w.AgreementID,
            w.CreatorUserID,
            w.Name,
            w.Description,
            w.Version,
            w.CreatedDate,
            w.UpdatedDate,
            w.Active,
            w.IsDeleted,
            s.WorkflowStatesId,
            s.State,
            s.BackgroundColor,
            s.FontColor
        INTO #WorkflowsTemp
        FROM Workflows w
        LEFT JOIN WorkflowStates s 
            ON s.WorkflowStatesId = w.WorkflowStatesId
        WHERE w.IsDeleted = 0 
          AND w.Active = 1
    ");

            var parameters = new DynamicParameters();

            // 🔹 Filtros
            if (workFlowId.HasValue)
            {
                sql.Append(" AND w.WorkFlowID = @WorkFlowID");
                parameters.Add("WorkFlowID", workFlowId.Value);
            }

            if (agreementId.HasValue)
            {
                sql.Append(" AND w.AgreementID = @AgreementID");
                parameters.Add("AgreementID", agreementId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                sql.Append(" AND s.State = @Status");
                parameters.Add("Status", status.Trim());
            }

            sql.Append(";");

            // 🔹 Devolver workflows
            sql.Append(@"
        SELECT * 
        FROM #WorkflowsTemp
        ORDER BY CreatedDate DESC;
    ");

            // 🔹 Query 2: Nacionalidades (usando Country)
            sql.Append(@"
        SELECT 
        n.WorkFlowID,
       c.countryId,
        c.NameESP AS NameCountry,
		n.WorkflowsNacionalidadesPermitidasId,
		n.active,
		n.isdeleted,
		c.NameESP
		
        FROM WorkflowsNacionalidadesPermitidas n
        INNER JOIN #WorkflowsTemp w 
            ON w.WorkFlowID = n.WorkFlowID
        INNER JOIN Country c
            ON c.CountryID = n.CountryID
        WHERE n.active = 1 AND n.IsDeleted = 0
    ");

            if (workFlowId.HasValue)
                sql.Append(" AND n.WorkFlowID = @WorkFlowID");

            sql.Append(";");

            // 🔹 Query 3: Ubicaciones (usando Country)
            sql.Append(@"
        SELECT 
            u.WorkFlowID,
            c.countryId,
            c.NameESP AS NameCountry,
            u.WorkflowsUbicacionesPermitidasId,
            u.active,
            u.isdeleted
        FROM WorkflowsUbicacionesPermitidas u
        INNER JOIN #WorkflowsTemp w 
            ON w.WorkFlowID = u.WorkFlowID
        INNER JOIN Country c
            ON c.CountryID = u.CountryID
        WHERE u.active = 1 AND u.IsDeleted = 0
    ");

            if (workFlowId.HasValue)
                sql.Append(" AND u.WorkFlowID = @WorkFlowID");

            sql.Append(";");

            // 🔹 Query 4: Documentos (usando DocumentType)
            sql.Append(@"
        SELECT 
            d.WorkFlowID,
            d.DocumentTypeId,
            dt.Name AS DocumentName,
            d.WorkflowsTipoDocumentosPermitidosId,
            d.active,
            d.isdeleted
        FROM WorkflowsTipoDocumentosPermitidos d
        INNER JOIN #WorkflowsTemp w 
            ON w.WorkFlowID = d.WorkFlowID
        INNER JOIN DocumentType dt
            ON dt.DocumentTypeId = d.DocumentTypeId
        WHERE d.active = 1 AND d.IsDeleted = 0
    ");

            if (workFlowId.HasValue)
                sql.Append(" AND d.WorkFlowID = @WorkFlowID");

            sql.Append(";");

            // 🔹 Ejecutar queries
            var multi = await _connection.QueryMultipleAsync(sql.ToString(), parameters);

            var workflows = (await multi.ReadAsync<WorkflowsEntry>()).ToList();
            var nationalities = (await multi.ReadAsync<NationalityByWorkflow>()).ToList();
            var ubications = (await multi.ReadAsync<UbicationsByWorkflow>()).ToList();
            var documents = (await multi.ReadAsync<DocumentTypeByWorkflow>()).ToList();

            // 🔹 Agrupar en memoria
            var nationalitiesMap = nationalities
                .GroupBy(x => x.WorkFlowID)
                .ToDictionary(g => g.Key, g => g.ToList());

            var ubicationsMap = ubications
                .GroupBy(x => x.WorkFlowID)
                .ToDictionary(g => g.Key, g => g.ToList());

            var documentsMap = documents
                .GroupBy(x => x.WorkFlowID)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 🔹 Mapear resultados
            foreach (var workflow in workflows)
            {
                workflow.NationalityByWorkFlow =
                    nationalitiesMap.GetValueOrDefault(workflow.WorkFlowID) ?? new List<NationalityByWorkflow>();

                workflow.Nationalities = string.Join(", ",
                    workflow.NationalityByWorkFlow.Select(x => x.NameCountry));

                workflow.UbicationsByWorkFlow =
                    ubicationsMap.GetValueOrDefault(workflow.WorkFlowID) ?? new List<UbicationsByWorkflow>();

                workflow.Ubications = string.Join(", ",
                    workflow.UbicationsByWorkFlow.Select(x => x.NameCountry));

                workflow.DocumentTypeByWorkFlow =
                    documentsMap.GetValueOrDefault(workflow.WorkFlowID) ?? new List<DocumentTypeByWorkflow>();
            }

            return workflows;
        }

        public async Task<bool> UpdateWorkflow(SaveWorflowRequest request)
        {
            var updates = new List<string>();
            var parameters = new DynamicParameters();

            if (request.CreatorUserID != null)
            {
                updates.Add("CreatorUserID = @CreatorUserID");
                parameters.Add("@CreatorUserID", request.CreatorUserID);
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                updates.Add("Name = @Name");
                parameters.Add("@Name", request.Name);
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                updates.Add("Description = @Description");
                parameters.Add("@Description", request.Description);
            }

            if (request.Version != null)
            {
                updates.Add("Version = @Version");
                parameters.Add("@Version", request.Version);
            }

                request.UpdatedDate = DateTime.Now;
                updates.Add("UpdatedDate = @UpdatedDate");
                parameters.Add("@UpdatedDate", request.UpdatedDate);
        

            if (request.Active != null)
            {
                updates.Add("Active = @Active");
                parameters.Add("@Active", request.Active);
            }

            if (request.IsDeleted != null)
            {
                updates.Add("IsDeleted = @IsDeleted");
                parameters.Add("@IsDeleted", request.IsDeleted);
            }

            if (request.WorkflowStatesId != null)
            {
                updates.Add("WorkflowStatesId = @WorkflowStatesId");
                parameters.Add("@WorkflowStatesId", request.WorkflowStatesId);
            }

            parameters.Add("@AgreementID", request.AgreementID);
            parameters.Add("@WorkFlowID", request.WorkFlowID);

            if (updates.Count == 0)
                return false;

            string sql = $@"
        UPDATE Workflows
        SET {string.Join(", ", updates)}
        WHERE WorkFlowID = @WorkFlowID
        AND AgreementID = @AgreementID";

            int rowsAffected = await _connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<WorkflowsNodeEntry>> GetAllWorkflowNode(int? workFlowNodeID = null, string? name = null)
        {
            string sqlQuery = @"
                SELECT 
                    WorkFlowNodeID,
                    Name,
                    Description,
                    Icon,
                    Html,
                    JsonProperties,
                    CreatedDate,
                    UpdatedDate,
                    Active,
                    IsDeleted
                FROM WorkflowsNode
                WHERE IsDeleted = 0 
                  AND (@WorkFlowNodeID IS NULL OR WorkFlowNodeID = @WorkFlowNodeID)
                  AND (@Name IS NULL OR Name = @Name)";

            var parameters = new { WorkFlowNodeID = workFlowNodeID, Name = name };

            var result = await _connection.QueryAsync<WorkflowsNodeEntry>(sqlQuery, parameters);
            return result;
        }

        public async Task<bool> UpdateWorkflowNode(DeleteWorflowNodeRequest request)
        {
            // 1. Asegurar que la conexión esté abierta antes de pedir la transacción
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            using var transaction = _connection.BeginTransaction();

            try
            {
                string sqlWorkflow = @"
            UPDATE Workflows
            SET 
                AgreementID = @AgreementID,
                UpdatedDate = @UpdatedDate,
                Active = @Active,
                IsDeleted = @IsDeleted
            WHERE WorkFlowID = @WorkFlowID";

                string sqlNacionalidades = @"
            UPDATE WorkflowsNacionalidadesPermitidas
            SET 
                Active = @Active,
                IsDeleted = @IsDeleted,
                UpdatedDate = @UpdatedDate
            WHERE WorkFlowID = @WorkFlowID";

                string sqlUbicaciones = @"
            UPDATE WorkflowsUbicacionesPermitidas
            SET 
                Active = @Active,
                IsDeleted = @IsDeleted,
                UpdatedDate = @UpdatedDate
            WHERE WorkFlowID = @WorkFlowID";

                string sqlTipoDocumentos = @"
            UPDATE WorkflowsTipoDocumentosPermitidos
            SET 
                Active = @Active,
                IsDeleted = @IsDeleted,
                UpdatedDate = @UpdatedDate
            WHERE WorkFlowID = @WorkFlowID";

                var parameters = new
                {
                    request.AgreementID,
                    UpdatedDate = DateTime.UtcNow,
                    request.Active,
                    request.IsDeleted,
                    request.WorkFlowID
                };

                // Pasar siempre el objeto 'transaction' en cada ExecuteAsync
                int rows1 = await _connection.ExecuteAsync(sqlWorkflow, parameters, transaction);
                int rows2 = await _connection.ExecuteAsync(sqlNacionalidades, parameters, transaction);
                int rows3 = await _connection.ExecuteAsync(sqlUbicaciones, parameters, transaction);
                int rows4 = await _connection.ExecuteAsync(sqlTipoDocumentos, parameters, transaction);

                transaction.Commit();

                // Retorna true si al menos se actualizó el workflow principal
                return rows1 > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<bool> SaveWorkflowNacionalidadesPermitidas(SaveWorkflowNacionalidadesPermitidasRequest request)
        {
            string sqlCommand = @"INSERT INTO WorkflowsNacionalidadesPermitidas
                                (WorkflowId, CountryId, CreatedDate, UpdatedDate, Active, IsDeleted) 
                                VALUES 
                                 (@WorkflowId, @CountryId, @CreatedDate, @UpdatedDate, @Active, @IsDeleted)";


            var result = await _connection.ExecuteAsync(sqlCommand, request);

            return result > 0;
        }

        public async Task<bool> SaveWorkflowTipoDocumento(SaveWorkflowTipoDocumentoRequest request)
        {
            string sqlCommand = @"INSERT INTO WorkflowsTipoDocumentosPermitidos
                                (WorkflowId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted) 
                                VALUES 
                                 (@WorkflowId, @DocumentTypeId, @CreatedDate, @UpdatedDate, @Active, @IsDeleted)";


            var result = await _connection.ExecuteAsync(sqlCommand, request);

            return result > 0;
        }

        public async Task<bool> SaveWorkflowUbicacionesPermitidas(SaveWorkflowUbicacionesPermitidasRequest request)
        {
            string sqlCommand = @"INSERT INTO WorkflowsUbicacionesPermitidas
                                (WorkflowId, CountryId, CreatedDate, UpdatedDate, Active, IsDeleted) 
                                VALUES 
                                 (@WorkflowId, @CountryId, @CreatedDate, @UpdatedDate, @Active, @IsDeleted)";


            var result = await _connection.ExecuteAsync(sqlCommand, request);

            return result > 0;
        }

        public async Task<IEnumerable<DocumentTypeByWorkflow>> GetDocumentosFromItem(int WorkFlowID)
        {
            string sqlCommand = @"
                SELECT WF.WorkFlowID,
                WF.AgreementID,
                WFTDP.WorkflowsTipoDocumentosPermitidosId,
                WFTDP.DocumentTypeId,
                OkSDocType.Code,
                OkSDocType.[Name] AS NameDocType,
                OkSDocType.[Length],
                OkSDocType.RegularExpression
                FROM Workflows AS WF
                INNER JOIN WorkflowsTipoDocumentosPermitidos AS WFTDP
                On (WFTDP.WorkflowId = WF.WorkFlowID)
                AND (WFTDP.Active = 1 and WFTDP.IsDeleted=0)
                INNER JOIN DocumentType AS OkSDocType
                On (WFTDP.DocumentTypeId = OkSDocType.DocumentTypeId)
                AND (OkSDocType.Active = 1 AND OkSDocType.IsDeleted=0)
                WHERE  WF.WorkFlowID =  @WorkFlowID
                and (WF.Active =1 and WF.IsDeleted=0)
                ";
            var parameters = new { WorkFlowID = WorkFlowID };

            var result = await _connection.QueryAsync<DocumentTypeByWorkflow>(sqlCommand, parameters);
            return result;
        }

        public async Task<IEnumerable<NationalityByWorkflow>> GetNationalitiesFromItem(int WorkFlowID)
        {
            string sqlCommand = @"SELECT WF.WorkFlowID,
                WF.AgreementID,
                WFNP.WorkflowsNacionalidadesPermitidasId,
                WFNP.CountryId,
                OkeyStCountry.NameESP AS NameCountry,
                OkeyStCountry.[Name] ,
                OkeyStCountry.Indicative,
                OkeyStCountry.RegionId,
                OkeyStCountry.Flag,
                OkeyStCountry.frecuentCountry,
                OkeyStRegion.[Name] AS NameRegion

                FROM Workflows AS WF
                INNER JOIN WorkflowsNacionalidadesPermitidas AS WFNP
                On (WFNP.WorkflowId = WF.WorkFlowID) 
                and (WFNP.Active = 1 AND WFNP.IsDeleted = 0)
                INNER JOIN Country AS OkeyStCountry 
                On (WFNP.CountryId = OkeyStCountry.CountryId) 
                and (OkeyStCountry.Active = 1 and OkeyStCountry.IsDeleted = 0)
                inner join Region as OkeyStRegion
                on (OkeyStCountry.RegionId = OkeyStRegion.RegionId)
                and (OkeyStRegion.Active = 1 and OkeyStRegion.IsDeleted = 0)

                WHERE  WF.WorkFlowID =  @WorkFlowID 
                and (WF.Active =1 and WF.IsDeleted=0)
                ";
            var parameters = new { WorkFlowID = WorkFlowID };

            var result = await _connection.QueryAsync<NationalityByWorkflow>(sqlCommand, parameters);
            return result;
        }

        public async Task<IEnumerable<UbicationsByWorkflow>> GetUbicacionesFromItem(int WorkFlowID)
        {
            string sqlCommand = @"
                SELECT WF.WorkFlowID,
                WF.AgreementID,
                WFUP.WorkflowsUbicacionesPermitidasId,
                WFUP.CountryId,
                OkeyStCountry.NameESP  AS NameCountry,
                OkeyStCountry.[Name] ,
                OkeyStCountry.Indicative,
                OkeyStCountry.RegionId,
                OkeyStCountry.Flag,
                OkeyStCountry.frecuentCountry,
                OkeyStRegion.[Name] AS NameRegion

                FROM Workflows AS WF
                INNER JOIN WorkflowsUbicacionesPermitidas AS WFUP
                On (WFUP.WorkflowId = WF.WorkFlowID)
                INNER JOIN Country AS OkeyStCountry
                On (OkeyStCountry.CountryId = WFUP.CountryId )
                inner join Region as OkeyStRegion
                on (OkeyStCountry.RegionId = OkeyStRegion.RegionId)
                and (OkeyStRegion.Active = 1 and OkeyStRegion.IsDeleted = 0)
                WHERE  WF.WorkFlowID =  @WorkFlowID
                AND (WF.Active = 1 AND WF.IsDeleted =0)
                AND (WFUP.Active = 1 AND WFUP.IsDeleted=0) 
                ";
            var parameters = new { WorkFlowID = WorkFlowID };

            var result = await _connection.QueryAsync<UbicationsByWorkflow>(sqlCommand, parameters);
            return result;
        }

        public async Task<IEnumerable<Domain.Parameters.DrawFlow.GetExistByNameAndGUIDResponse>> GetExistByNameAndGUID(
            Guid AgreementID,
            string Name,
            int? WorkflowID = null  
        )
        {

            string sqlCommand = @"SELECT 
                            COUNT(*) AS Total,
                            CASE 
                                WHEN COUNT(*) > 0 THEN CAST(1 AS bit)   -- TRUE → Ya existe
                                ELSE CAST(0 AS bit)                     -- FALSE → No existe
                            END AS YaExiste
                        FROM WORKFLOWS
                        WHERE NAME = @Name
                          AND AgreementID = @AgreementID";


            if (WorkflowID.HasValue)
            {
                sqlCommand += " AND WorkFlowID != @WorkflowID";
            }

            var parameters = new { AgreementID, Name, WorkflowID };

            var result = await _connection.QueryAsync<Domain.Parameters.DrawFlow.GetExistByNameAndGUIDResponse>(sqlCommand, parameters);
            return result;
        }


        public async Task<bool> DuplicateSQLWorkflow(WorkflowsEntry request)
        {

            string sqlCommand = "sp_DuplicateWorkflow";

            var parameters = new
            {
                WorkflowIdOld = request.WorkFlowID, 
                AgreementId = request.AgreementID
            };

            var result = await _connection.ExecuteAsync(
                sqlCommand,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return result >= 0 || result == -1;
        }
      
        public async Task<List<NationalityByWorkflow>> GetNationalitiesByWorkflow(int workflowId)
        {
            const string sql = @"
        SELECT 
            WorkFlowID, 
            CountryID, 
            Active,
            IsDeleted 
        FROM WorkflowsNacionalidadesPermitidas 
        WHERE WorkFlowID = @workflowId"; 

            var result = await _connection.QueryAsync<NationalityByWorkflow>(sql, new { workflowId });
            return result.ToList();
        }

        public async Task<bool> UpdateNationalityStatus(int workflowId, int countryId, bool status, bool isDeleted)
        {
            const string sql = @"
        UPDATE WorkflowsNacionalidadesPermitidas 
        SET Active = @activeBit, 
            IsDeleted = @deletedBit, 
            UpdatedDate = GETDATE() 
        WHERE WorkFlowID = @workflowId 
          AND CountryID = @countryId";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                workflowId,
                countryId,
                activeBit = status ? 1 : 0,    // Si status es true, Active = 1
                deletedBit = isDeleted ? 1 : 0 // Si isDeleted es true, IsDeleted = 1
            });

            return rowsAffected > 0;
        }

        public async Task<bool> InsertNationality(int workflowId, int countryId)
        {
            const string sql = @"
        INSERT INTO WorkflowsNacionalidadesPermitidas 
        (
            WorkFlowID, 
            CountryID, 
            Active, 
            CreatedDate
        )
        VALUES 
        (
            @workflowId, 
            @countryId, 
            1, 
            GETDATE()
        )";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                workflowId,
                countryId
            });

            return rowsAffected > 0;
        }

        public async Task<List<DocumentTypeByWorkflow>> GetDocumentsByWorkflow(int workflowId)
        {
            const string sql = @"
        SELECT 
            WorkFlowID, 
            DocumentTypeID, 
            Active 
        FROM WorkflowsTipoDocumentosPermitidos 
        WHERE WorkFlowID = @workflowId";

            var result = await _connection.QueryAsync<DocumentTypeByWorkflow>(sql, new { workflowId });
            return result.ToList();
        }

        public async Task<bool> UpdateDocumentStatus(int workflowId, int documentTypeId, bool status, bool isDeleted)
        {
            const string sql = @"
        UPDATE WorkflowsTipoDocumentosPermitidos 
        SET Active = @activeBit, 
            IsDeleted = @deletedBit, 
            UpdatedDate = GETDATE() 
        WHERE WorkFlowID = @workflowId 
          AND DocumentTypeID = @documentTypeId";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                workflowId,
                documentTypeId,
                activeBit = status ? 1 : 0,    // Si status es true, Active = 1
                deletedBit = isDeleted ? 1 : 0 // Si isDeleted es true, IsDeleted = 1
            });

            return rowsAffected > 0;
        }
        public async Task<bool> InsertDocument(int workflowId, int documentTypeId)
        {
            const string sql = @"
        INSERT INTO WorkflowsTipoDocumentosPermitidos 
        (
            WorkFlowID, 
            DocumentTypeID, 
            Active, 
            CreatedDate
        )
        VALUES 
        (
            @workflowId, 
            @documentTypeId, 
            1, 
            GETDATE()
        )";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                workflowId,
                documentTypeId
            });

            return rowsAffected > 0;
        }

        public async Task<List<UbicationsByWorkflow>> GetLocationsByWorkflow(int workflowId)
        {
            const string sql = @"
        SELECT 
            WorkFlowID, 
            CountryID, 
            Active 
        FROM WorkflowsUbicacionesPermitidas 
        WHERE WorkFlowID = @workflowId";

            var result = await _connection.QueryAsync<UbicationsByWorkflow>(sql, new { workflowId });
            return result.ToList();
        }

        public async Task<bool> UpdateLocationStatus(int workflowId, int countryId, bool status, bool isDeleted)
        {
            const string sql = @"
        UPDATE WorkflowsUbicacionesPermitidas 
        SET Active = @activeBit, 
            IsDeleted = @deletedBit, 
            UpdatedDate = GETDATE() 
        WHERE WorkFlowID = @workflowId 
          AND CountryID = @countryId";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                workflowId,
                countryId,
                activeBit = status ? 1 : 0,
                deletedBit = isDeleted ? 1 : 0
            });

            return rowsAffected > 0;
        }

        public async Task<bool> InsertLocation(int workflowId, int countryId)
        {
            const string sql = @"
        INSERT INTO WorkflowsUbicacionesPermitidas 
        (
            WorkFlowID, 
            CountryID, 
            Active, 
            CreatedDate
        )
        VALUES 
        (
            @workflowId, 
            @countryId, 
            1, 
            GETDATE()
        )";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                workflowId,
                countryId
            });

            return rowsAffected > 0;
        }

        public async Task<bool> ArchiveWorkflow(ArchiveWorkflowRequest request)
        {
            const string sql = @"
        UPDATE W
        SET
            StatusPreArchived = W.WorkflowStatesId,
            WorkflowStatesId = NewState.WorkflowStatesId,
            UpdatedDate = GETDATE(),
            ArchivedDate = GETDATE()
        FROM Workflows W
        CROSS APPLY (
            SELECT TOP (1) WS.WorkflowStatesId
            FROM WorkflowStates WS
            WHERE WS.Active = 1
              AND WS.IsDeleted = 0
              AND UPPER(WS.[State]) = 'ARCHIVADO'
            ORDER BY WS.WorkflowStatesId ASC
        ) NewState
        WHERE W.WorkFlowID = @workflowId;
    ";

            var rowsAffected = await _connection.ExecuteAsync(
                sql,
                new { workflowId = request.WorkFlowID }
            );

            return rowsAffected > 0;
        }

        public async Task<bool> PublicarWorkflow(PublicarWorkflowRequest request)
        {
            try
            {
                var parameters = new
                {
                    AgreementId = request.AgreementID,
                    WorkflowId = request.WorkFlowID
                };

                await _connection.ExecuteAsync(
                    "sp_SetWorkflowPublicado",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return true; 
            }
            catch (Exception ex)
            {            
                return false;
            }
        }



        public async Task<bool> PauseWorkflow(PauseWorkflowRequest request)
        {
            const string sql = @"
            update Workflows
            set WorkflowStatesId = (select top 1 WorkflowStatesId  from WorkflowStates where [State]  = 'Pausado' and active = 1 and IsDeleted = 0), 
            UpdatedDate = GETDATE()
            where WorkflowId = @workflowId  and active = 1 and IsDeleted = 0 ";

            var rowsAffected = await _connection.ExecuteAsync(
                sql,
                new { workflowId = request.WorkFlowID }
            );

            return rowsAffected > 0;
        }

        public async Task<bool> DesarchiveWorkflow(DesarchiveWorkflowRequest request)
        {
            const string sql = @"
        UPDATE W
        SET
            WorkflowStatesId = W.StatusPreArchived,
            StatusPreArchived = 0,
            ArchivedDate = '1900-01-01',
            UpdatedDate = GETDATE()
        FROM Workflows W
        WHERE W.WorkFlowID = @workflowId
          AND W.StatusPreArchived <> 0
          AND W.ArchivedDate <> '1900-01-01'
          AND W.Active = 1
          AND W.IsDeleted = 0;
    ";

            var rowsAffected = await _connection.ExecuteAsync(
                sql,
                new { workflowId = request.WorkFlowID }
            );

            return rowsAffected > 0;
        }

    }
}
