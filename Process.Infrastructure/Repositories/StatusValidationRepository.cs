using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class StatusValidationRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IStatusValidationRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<StatusValidation?> GetStatusValidationByStatusCodeAsync(int statusCode)
        {
            string sql = @"
                SELECT 
                    [StatusValidationId],
                    [StatusCode],
                    [Description],
                    [CreatedDate],
                    [UpdatedDate],
                    [Active],
                    [IsDeleted]
                FROM [dbo].[StatusValidation]
                WHERE StatusCode = @statusCode
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<StatusValidation>(sql, new { statusCode });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
