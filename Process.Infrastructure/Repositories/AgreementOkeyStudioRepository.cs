using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class AgreementOkeyStudioRepository(SQLServerConnectionFactory connectionFactory) : IAgreementOkeyStudioRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<AgreementOkeyStudio?> GetAgreementById(long id)
        {
            string sql = @"
                SELECT 
                    [AgreementId],[AgreementGUID],[ClientId],[Status],[Name],[ATDPToken],[CreatorUserId],
                    [CreatedDate],[UpdatedDate],[Active],[IsDeleted],[UserReconoserId],[PasswordReconoserId],[PlatformConnection],
                    [ChangeUrl],[BaseUrlReconoser1],[BaseUrlReconoser2]
                FROM [dbo].[Agreement]
                WHERE AgreementId = @id
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AgreementOkeyStudio>(sql, new { id });
        }

        public async Task<AgreementOkeyStudio?> GetAgreementByGuid(Guid agreementGuid)
        {
            string sql = @"
                SELECT 
                    [AgreementId],[AgreementGUID],[ClientId],[Status],[Name],[ATDPToken],[CreatorUserId],
                    [CreatedDate],[UpdatedDate],[Active],[IsDeleted],[UserReconoserId],[PasswordReconoserId],[PlatformConnection],
                    [ChangeUrl],[BaseUrlReconoser1],[BaseUrlReconoser2]
                FROM [dbo].[Agreement]
                WHERE AgreementGUID = @agreementGuid
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AgreementOkeyStudio>(sql, new { agreementGuid });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }

        public async Task<IReadOnlyList<AgreementOkeyStudio>> GetAgreementsByClientId(Guid ClientToken)
        {
            const string sql = @" SELECT 
                                        a.AgreementId,
                                        a.AgreementGUID,
                                        a.Status,
                                        a.Name,
                                        a.ATDPToken,
                                        a.UserReconoserId,
                                        a.CreatorUserId,
                                        a.CreatedDate,
                                        a.UpdatedDate,
                                        a.Active,
                                        a.IsDeleted,
                                        c.ClientToken
                                    FROM 
                                        Agreement a
                                    INNER JOIN 
                                        Client c ON a.ClientId = c.ClientId
                                    WHERE 
	                                    c.ClientToken = @ClientToken AND
                                        a.Active = 1 AND 
                                        a.IsDeleted = 0 AND
	                                    c.Active = 1 AND 
                                        c.IsDeleted = 0
                                    ORDER BY 
                                        a.Name; 
            ";

            using var connection = CreateConnection();
            var result = await connection.QueryAsync<AgreementOkeyStudio>(sql, new {  ClientToken });
            return result.ToList();
        }

    }
}
