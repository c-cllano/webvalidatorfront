using Dactyloscopy.Domain.Entities;
using Dactyloscopy.Domain.Repositories;
using Dactyloscopy.Infrastructure.Data;
using Dapper;
using System.Data;

namespace Dactyloscopy.Infrastructure.Repositories
{
    public class StatusForensicRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IStatusForensicRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<StatusForensic?> GetStatusByIdAsync(long statusForensicId)
        {
            string sql = @"
                SELECT 
                    [StatusForensicId],[Description],[CreatedDate],[UpdatedDate],
                    [Active],[IsDeleted]
                FROM [dbo].[StatusForensic]
                WHERE StatusForensicId = @statusForensicId
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<StatusForensic>(sql, new { statusForensicId });
        }

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
            return _factory.CreateConnection();
        }
    }
}
