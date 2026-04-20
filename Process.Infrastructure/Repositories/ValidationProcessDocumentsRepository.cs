using Dapper;
using Process.Domain.Entities;
using Process.Domain.Parameters.ProcessDocuments;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using System.Text.Json;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ValidationProcessDocumentsRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IValidationProcessDocumentsRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task SaveValidationProcessDocumentsAsync(
            long validationProcessId,
            ProcessDocumentsRequest processDocumentsRequest
        )
        {
            long? validationProcessDocumentsId = await GetValidationProcessDocumentsByValidationProcessIdAsync(
                validationProcessId
            );

            string trazability = JsonSerializer.Serialize(processDocumentsRequest);

            if (validationProcessDocumentsId == null)
            {
                await CreateEntityAsync(validationProcessId, trazability);
            }
            else
            {
                await UpdateEntityAsync(validationProcessDocumentsId.Value, trazability);
            }
        }

        private async Task<ValidationProcessDocuments> CreateEntityAsync(
            long validationProcessId,
            string trazability
        )
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[ValidationProcessDocuments](
                    [ValidationProcessId]
                   ,[Trazability]
                   ,[CreatedDate]
                   ,[Active]
                   ,[IsDeleted]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @validationProcessId
                   ,JSON_QUERY(CONCAT('{""Captures"":[', @trazability, ']}'))
                   ,@CreatedDate
                   ,@Active
                   ,@IsDeleted
                )
            ";

            using var connection = CreateConnection();

            return await connection.QuerySingleAsync<ValidationProcessDocuments>(
                sqlCommand,
                new
                {
                    validationProcessId,
                    trazability,
                    CreatedDate = DateTime.UtcNow.AddHours(-5),
                    Active = 1,
                    IsDeleted = 0
                }
            );
        }

        private async Task<ValidationProcessDocuments> UpdateEntityAsync(
            long validationProcessDocumentsId,
            string trazability
        )
        {
            string sqlCommand = @"
                UPDATE [dbo].[ValidationProcessDocuments]
                SET 
                    [Trazability] =
                        CASE
                            WHEN [Trazability] IS NULL OR LTRIM(RTRIM([Trazability])) = '' 
                                THEN JSON_QUERY(
                                    CONCAT(
                                        '{""Captures"":[', 
                                        @trazability, 
                                        ']}'
                                    )
                                )
                            WHEN ISJSON([Trazability]) = 1
                                THEN JSON_MODIFY(
                                    [Trazability],
                                    'append $.Captures',
                                    JSON_QUERY(@trazability)
                                )
                            ELSE JSON_QUERY(
                                CONCAT(
                                    '{""Captures"":[', 
                                    @trazability, 
                                    ']}'
                                )
                            )
                        END,
                    [UpdatedDate] = @UpdatedDate
                OUTPUT INSERTED.*
                WHERE ValidationProcessDocumentsId = @validationProcessDocumentsId
            ";

            using var connection = CreateConnection();

            return await connection.QuerySingleAsync<ValidationProcessDocuments>(
                sqlCommand,
                new
                {
                    trazability,
                    UpdatedDate = DateTime.UtcNow.AddHours(-5),
                    validationProcessDocumentsId
                }
            );
        }

        private async Task<long?> GetValidationProcessDocumentsByValidationProcessIdAsync(long validationProcessId)
        {
            long? validationProcessDocumentsId = null!;

            string sql = @"
                SELECT TOP 1
                    ValidationProcessDocumentsId
                FROM [dbo].[ValidationProcessDocuments]
                WHERE ValidationProcessId = @validationProcessId
                ORDER BY CreatedDate DESC
            ";

            using var connection = CreateConnection();

            var entity = await connection.QueryFirstOrDefaultAsync<ValidationProcessDocuments>(sql, new { validationProcessId });

            if (entity != null)
                validationProcessDocumentsId = entity.ValidationProcessDocumentsId;

            return validationProcessDocumentsId;
        }

        public async Task<List<ValidationProcessDocuments>> GetDocumentsByValidationProcessIdAsync(long validationProcessId)
        {
            string sql = @"
                SELECT 
                    [ValidationProcessDocumentsId]
                   ,[ValidationProcessId]
                   ,[Trazability]
                   ,[CreatedDate]
                   ,[Active]
                   ,[IsDeleted]
                FROM [dbo].[ValidationProcessDocuments]
                WHERE   [ValidationProcessId] = @validationProcessId
            ";
            using var connection = CreateConnection();
            return (await connection.QueryAsync<ValidationProcessDocuments>(sql, new { validationProcessId })).ToList();
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
