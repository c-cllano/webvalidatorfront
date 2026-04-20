using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class DocumentTypeCaptureRepository(SQLServerConnectionFactory factory) : IDocumentTypeCaptureRepository
    {
        private readonly SQLServerConnectionFactory _factory = factory;

        public async Task<DocumentTypeCapture?> GetByCodeOrDefaultAsync(string code)
        {
            var sql = @" 
                SELECT TOP 1 dct.* 
                FROM DocumentTypeCapture dct 
                INNER JOIN DocumentType dt ON dct.DocumentTypeId = dt.DocumentTypeId 
                WHERE dt.Code = @Code

                UNION ALL 

                SELECT TOP 1 dct.* 
                FROM DocumentTypeCapture dct 
                WHERE NOT EXISTS ( 
                    SELECT 1 
                    FROM DocumentTypeCapture dct2 
                    INNER JOIN DocumentType dt2 ON dct2.DocumentTypeId = dt2.DocumentTypeId 
                    WHERE dt2.Code = @Code 
                ) 
                ORDER BY DocumentTypeCaptureId;
            ";                                  

            using var conn = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await conn.QueryFirstOrDefaultAsync<DocumentTypeCapture>(sql, new { Code = code });
        }
    }
}