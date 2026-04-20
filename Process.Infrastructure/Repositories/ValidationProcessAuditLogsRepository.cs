using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ValidationProcessAuditLogsRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IValidationProcessAuditLogsRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task SaveAuditLogsAsync(ValidationProcessAuditLogs auditLogs)
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[ValidationProcessAuditLogs](
                    [ValidationProcessId]
                   ,[Url]
                   ,[Method]
                   ,[RequestBody]
                   ,[ResponseBody]
                   ,[StatusCode]
                   ,[DurationMs]
                   ,[Exception]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @ValidationProcessId
                   ,@Url
                   ,@Method
                   ,@RequestBody
                   ,@ResponseBody
                   ,@StatusCode
                   ,@DurationMs
                   ,@Exception
                )
            ";

            using var connection = CreateConnection();
            await connection.QuerySingleAsync<ValidationProcessAuditLogs>(sqlCommand, auditLogs);
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
