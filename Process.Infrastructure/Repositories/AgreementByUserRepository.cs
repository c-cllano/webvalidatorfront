using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class AgreementByUserRepository(SQLServerConnectionFactory connectionFactory) : IAgreementByUserRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<long> AddAsync(AgreementByUser agreementByUser)
        {
            var sql = @"INSERT INTO AgreementByUser (UserId, AgreementId, CreatorUserId, CreatedDate, UpdateDate, Active, IsDeleted)
                        VALUES (@UserId, @AgreementId, @CreatedUserId, @CreatedDate, @UpdateDate, @Active, @IsDelete);
                        SELECT CAST(SCOPE_IDENTITY() as bigint);";
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(sql, agreementByUser);
        }

        public async Task<AgreementByUser?> GetByUserIdAgreementAsync(int userId, int agreementId)
        {
            var sql = @"SELECT 
                         a.Name as AgreementName,
                         a.AgreementGUID,
                         a.Active,
                         au.AgreementByUserId,
                         au.AgreementId,
                         au.UserId
                        FROM AgreementByUser as au 
                          INNER JOIN Agreement as a	
                            on au.AgreementId = a.AgreementId
                            and a.IsDeleted = 0 and au.Active = 1
                            and au.IsDeleted = 0
                        WHERE au.UserId = @UserId and au.AgreementId = @AgreementId";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AgreementByUser>(sql, new { UserId = userId, AgreementId = agreementId });
        }

        public async Task<IEnumerable<AgreementByUser>> GetByUserIdAsync(int userId)
        {
            var sql = @"SELECT  
                            a.Name AS AgreementName,
                            a.AgreementGUID,
                            a.Active,
                            au.RoleAgreementByUserId,
                            au.AgreementId,
                            au.UserId,
                            CAST(c.ClientToken AS VARCHAR(50)) AS ClientToken
                        FROM RoleAgreementByUser AS au
                        INNER JOIN Agreement AS a
                            ON au.AgreementId = a.AgreementId
                           AND a.IsDeleted = 0
                           AND au.Active = 1
                           AND au.IsDeleted = 0
                        INNER JOIN Client AS c
                            ON c.ClientId = a.ClientId
                        INNER JOIN Role AS rol
                            ON rol.RoleId = au.RoleId  AND rol.Active = 1 AND rol.IsDeleted = 0
                        WHERE au.UserId = @UserId";
            using var connection = CreateConnection();
            return await connection.QueryAsync<AgreementByUser>(sql, new { UserId = userId });
        }

        public async Task UpdateUserAgreements(int userId, IEnumerable<int> agreementIds, long updaterUserId)
        {
            var ids = (agreementIds ?? []).Distinct().Where(a => a > 0).ToArray();
            
            using var connection = CreateConnection();
            connection.Open();
            using var tx = connection.BeginTransaction();

            try
            {
               
                var deleteSql = @"UPDATE AgreementByUser
                                  SET IsDeleted = 1, UpdateDate = GETDATE()
                                  WHERE UserId = @UserId 
                                    AND IsDeleted = 0
                                    AND (@IdsCount = 0 OR AgreementId NOT IN @Ids)";
                await connection.ExecuteAsync(deleteSql, new { UserId = userId, Ids = ids, IdsCount = ids.Length }, tx);

              
                var reactivateSql = @"UPDATE AgreementByUser
                                      SET IsDeleted = 0, Active = 1, UpdateDate = GETDATE()
                                      WHERE UserId = @UserId 
                                        AND AgreementId IN @Ids 
                                        AND IsDeleted = 1";
                await connection.ExecuteAsync(reactivateSql, new { UserId = userId, Ids = ids }, tx);

              
                var insertSql = @"INSERT INTO AgreementByUser (UserId, AgreementId, CreatorUserId, CreatedDate, UpdateDate, Active, IsDeleted)
                                  SELECT @UserId, @AgreementId, @CreatorUserId, GETDATE(), NULL, 1, 0
                                  WHERE NOT EXISTS (
                                      SELECT 1 FROM AgreementByUser
                                      WHERE UserId = @UserId AND AgreementId = @AgreementId AND IsDeleted = 0
                                  );";
                
                foreach (var agreementId in ids)
                {
                    await connection.ExecuteAsync(insertSql, new { 
                        UserId = userId, 
                        AgreementId = agreementId, 
                        CreatorUserId = updaterUserId 
                    }, tx);
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
