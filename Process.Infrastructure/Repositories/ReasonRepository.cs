using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ReasonRepository(SQLServerConnectionFactory connectionFactory) : IReasonRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<Reason?> GetReasonById(long id)
        {
            string sql = @"
                SELECT 
                    [ReasonID],
                    [Description],
                    [CreatedDate],
                    [UpdatedDate],
                    [Approved],
                    [Active],
                    [IsDelete]
                FROM [dbo].[Reason]
                WHERE ReasonID = @id
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Reason>(sql, new { id });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
