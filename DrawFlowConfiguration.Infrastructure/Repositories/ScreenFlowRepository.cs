using System.Data;
using Dapper;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Request;
using DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Response;
using DrawFlowConfiguration.Domain.Repositories;
using DrawFlowConfiguration.Infrastructure.Data;

namespace DrawFlowConfiguration.Infrastructure.Repositories
{


    public class ScreenFlowRepository(SQLServerConnectionFactory connectionFactory) : IScreenFlowRepository
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();

        public async Task<bool> SaveScreenFlow
            (ScreenFlowRequest request)
        {
            string sqlCommand = @" INSERT INTO ScreenFlow 
                                (AgreementID,SelectedIdWorkflow,ContScreenFlow, OperationScreenFlowID, StateScreenFlow, CreatorUserID, CreatedDate, UpdatedDate, Active, IsDeleted)
                                VALUES
                                 (@AgreementID,@SelectedIdWorkflow,@ContScreenFlow, @OperationScreenFlowID, @StateScreenFlow, @CreatorUserID, @CreatedDate, @UpdatedDate, @Active, @IsDeleted)";

            var result = await _connection.ExecuteAsync(sqlCommand, request);

            return result > 0;
        }



        public async Task<IEnumerable<ScreenFlowResponse>> GetAllScreenFlow()
        {
            string sqlQuery = @"
                                SELECT 
                                    ScreenFlowID,
                                    AgreementID,
                                    SelectedIdWorkflow,
                                    ContScreenFlow,
                                    OperationScreenFlowID,
                                    StateScreenFlow,
                                    CreatorUserID,
                                    CreatedDate,
                                    UpdatedDate,
                                    Active,
                                    IsDeleted
                                FROM ScreenFlow
                                WHERE IsDeleted = 0";


            var result = await _connection.QueryAsync<ScreenFlowResponse>(sqlQuery);
            return result;
        }

        
        
        public async Task<IEnumerable<ScreenFlowResponse>> GetScreenFlowByFilter(int? screenFlowId = null, Guid? agreementId = null, int? selectedIdWorkflow = null)
        {
            var sqlBuilder = new System.Text.StringBuilder(@"
                SELECT 
                    ISNULL(CAST(ROW_NUMBER() OVER (ORDER BY ScreenFlowID) AS INT), 0) AS RowNum,
                    ScreenFlowID,
                    AgreementID,
                    SelectedIdWorkflow,
                    ContScreenFlow,
                    OperationScreenFlowID,
                    StateScreenFlow,
                    CreatorUserID,
                    ScreenFlow.CreatedDate,
                    ScreenFlow.UpdatedDate,
                    ScreenFlow.Active,
                    ScreenFlow.IsDeleted
                FROM ScreenFlow
                WHERE 
                    (ScreenFlow.IsDeleted = 0 AND ScreenFlow.Active = 1)
            ");

            var parameters = new DynamicParameters();

            if (screenFlowId.HasValue)
            {
                sqlBuilder.Append(" AND ScreenFlowID = @ScreenFlowID");
                parameters.Add("ScreenFlowID", screenFlowId.Value);
            }

            if (agreementId.HasValue)
            {
                sqlBuilder.Append(" AND AgreementID = @AgreementID");
                parameters.Add("AgreementID", agreementId.Value);
            }
            if (agreementId.HasValue)
            {
                sqlBuilder.Append(" AND SelectedIdWorkflow = @SelectedIdWorkflow");
                parameters.Add("SelectedIdWorkflow", selectedIdWorkflow!.Value);
            }

            sqlBuilder.Append(" ORDER BY CreatedDate DESC");

            var result = await _connection.QueryAsync(sqlBuilder.ToString(), parameters);

            var ScreenFlow = result.Select(row => new ScreenFlowResponse
            {
                ScreenFlowID = row.ScreenFlowID,
                SelectedIdWorkflow = row.SelectedIdWorkflow,
                ContScreenFlow = row.ContScreenFlow,
                AgreementID = row.AgreementID,
                OperationScreenFlowID = row.OperationScreenFlowID,
                StateScreenFlow = row.StateScreenFlow,
                CreatorUserID = row.CreatorUserID,
                CreatedDate = row.CreatedDate,
                UpdatedDate = row.UpdatedDate,
                Active = row.Active,
                IsDeleted = row.IsDeleted
            }).ToList();

            return ScreenFlow;
        }



        public async Task<bool> UpdateScreenFlow(ScreenFlowResponse request)
        {

            string sql = @"
                            UPDATE ScreenFlow
                            SET 
                                AgreementID = @AgreementID,
                                SelectedIdWorkflow =@SelectedIdWorkflow,
                                ContScreenFlow= @ContScreenFlow,
                                OperationScreenFlowID = @OperationScreenFlowID,
                                StateScreenFlow = @StateScreenFlow,
                                CreatorUserID = @CreatorUserID,
                                UpdatedDate = @UpdatedDate,
                                Active = @Active,
                                IsDeleted = @IsDeleted
                            WHERE ScreenFlowID = @ScreenFlowID";

            var parameters = new
            {
                request.ScreenFlowID,
                request.AgreementID,
                request.OperationScreenFlowID,
                request.StateScreenFlow,
                request.CreatorUserID,
                UpdatedDate = DateTime.UtcNow, // o request.UpdatedDate si viene desde frontend
                request.Active,
                request.IsDeleted,
            };

            int rowsAffected = await _connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }


      



    }
}
