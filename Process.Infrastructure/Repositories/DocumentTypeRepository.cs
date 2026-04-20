using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class DocumentTypeRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IDocumentTypeRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<DocumentType?> GetDocumentTypeById(int documentTypeId)
        {
            string sql = @"
                SELECT 
                    [DocumentTypeId],[Code],[Name],[Length],[RegularExpression],
                    [CreatedDate],[UpdatedDate],[Active],[IsDeleted]
                FROM [dbo].[DocumentType]
                WHERE DocumentTypeId = @documentTypeId
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<DocumentType>(sql, new { documentTypeId });
        }

        public async Task<DocumentType?> GetDocumentTypeByCode(string code)
        {
            string sql = @"
                SELECT 
                    [DocumentTypeId],[Code],[Name],[Length],[RegularExpression],
                    [CreatedDate],[UpdatedDate],[Active],[IsDeleted]
                FROM [dbo].[DocumentType]
                WHERE Code = @code
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<DocumentType>(sql, new { code });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
