using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class StatusForensicRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IStatusForensicRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<StatusForensic?> GetStatusByDescriptionAsync(string description)
        {
            string sql = @"
                SELECT 
                    [StatusForensicId],[Description],[CreatedDate],[UpdatedDate],
                    [Active],[IsDeleted]
                FROM [dbo].[StatusForensic]
                WHERE Description = @description
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<StatusForensic>(sql, new { description });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
