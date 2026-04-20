using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ValidationProcessExecutionRepository(SQLServerConnectionFactory connectionFactory) : IValidationProcessExecutionRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<ValidationProcessExecution?> GetValidationProcessExecutionById(long id)
        {
            string sql = @"
                SELECT 
                    [ValidationProcessExecutionId],[ValidationProcessId],[StartDate],[FinishDate],[LastStep],[Trazability]
                    ,[CreatedDate],[UpdatedDate],[Active],[IsDeleted]
                FROM [dbo].[ValidationProcessExecution]
                WHERE ValidationProcessExecutionId = @id
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ValidationProcessExecution>(sql, new { id });
        }

        public async Task<ValidationProcessExecution?> GetValidationProcessExecutionByValidationProcessIdAsync(long validationProcessId)
        {
            string sql = @"
                SELECT 
                    [ValidationProcessExecutionId],[ValidationProcessId],[StartDate],[FinishDate],[LastStep],[Trazability]
                    ,[CreatedDate],[UpdatedDate],[Active],[IsDeleted]
                FROM [dbo].[ValidationProcessExecution]
                WHERE Active = 1 AND ValidationProcessId = @validationProcessId
                ORDER BY CreatedDate DESC
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ValidationProcessExecution>(sql, new { validationProcessId });
        }

        public async Task<ValidationProcessExecution> SaveValidationProcessExecutionAsync(ValidationProcessExecution validationProcessExecution)
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[ValidationProcessExecution](
                    [ValidationProcessId],[StartDate],[FinishDate],[LastStep],[Trazability],[CreatedDate]
                   ,[UpdatedDate],[Active],[IsDeleted]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @ValidationProcessId,@StartDate,@FinishDate,@LastStep,@Trazability,@CreatedDate
                    ,@UpdatedDate,@Active,@IsDeleted
                )
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<ValidationProcessExecution>(sqlCommand, validationProcessExecution);
        }

        public async Task<ValidationProcessExecution> UpdateValidationProcessExecutionAsync(ValidationProcessExecution validationProcessExecution)
        {
            string sqlCommand = @"
                UPDATE [dbo].[ValidationProcessExecution]
                SET 
                    [ValidationProcessId] = @ValidationProcessId,
                    [StartDate] = @StartDate,
                    [FinishDate] = @FinishDate,
                    [LastStep] = @LastStep,
                    [Trazability] = 
                        CASE
                            WHEN [Trazability] IS NULL OR LTRIM(RTRIM([Trazability])) = '' THEN CONCAT('[', @Trazability, ']')
                            WHEN LEFT(LTRIM([Trazability]), 1) = '[' THEN LEFT([Trazability], LEN([Trazability]) - 1) + ',' + @Trazability + ']'
                            ELSE CONCAT('[', [Trazability], ',', @Trazability, ']')
                        END,
                    [CreatedDate] = @CreatedDate,
                    [UpdatedDate] = @UpdatedDate,
                    [Active] = @Active,
                    [IsDeleted] = @IsDeleted
                OUTPUT INSERTED.*
                WHERE [ValidationProcessExecutionId] = @ValidationProcessExecutionId
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<ValidationProcessExecution>(sqlCommand, validationProcessExecution);
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
