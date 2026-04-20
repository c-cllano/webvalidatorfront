using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class RoleByUserRepository(SQLServerConnectionFactory connectionFactory) : IRoleByUserRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;
        
        public async Task<long> AddAsync(RoleByUser roleByUser)
        {
            var sql = @"INSERT INTO [dbo].[RoleByUser] (RoleId, UserId, CreatorUserId, CreatedDate, Active, IsDeleted)
                        VALUES (@RoleId, @UserId, @CreatorUserId, @CreatedDate, @Active, @IsDeleted);
                        SELECT CAST(SCOPE_IDENTITY() as bigint);";
            
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(sql, roleByUser);
        }

        public async Task<IEnumerable<RoleByUser>> GetByUserIdAsync(int userId)
        {
            var sql = @"SELECT RoleByUserId, RoleId, UserId, CreatorUserId, CreatedDate, Active, IsDeleted
                        FROM [dbo].[RoleByUser]
                        WHERE UserId = @UserId AND IsDeleted = 0";
            
            using var connection = CreateConnection();
            return await connection.QueryAsync<RoleByUser>(sql, new { UserId = userId });
        }

        public async Task<RoleByUser?> GetByUserIdAndRoleIdAsync(int userId, long roleId)
        {
            var sql = @"SELECT RoleByUserId, RoleId, UserId, CreatorUserId, CreatedDate, Active, IsDeleted
                        FROM [dbo].[RoleByUser]
                        WHERE UserId = @UserId AND RoleId = @RoleId AND IsDeleted = 0";
            
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<RoleByUser>(sql, new { UserId = userId, RoleId = roleId });
        }

        public async Task<IEnumerable<UserRoleAgreementDto>> GetUserRolesAndAgreementsAsync(int userId)
        {
            var sql = @"
                SELECT DISTINCT
                        r.RoleId,
                        r.Name AS RoleName,
                        ag.Name AS AgreementName,
                        ag.AgreementId
                    FROM RoleByUser rbu
                    INNER JOIN Role r 
                        ON rbu.RoleId = r.RoleId 
                       AND r.IsDeleted = 0 and rbu.IsDeleted = 0
                    INNER JOIN RoleByAgreement rba 
                        ON r.RoleId = rba.RoleId
                       AND rba.IsDeleted = 0
                    INNER JOIN AgreementByUser abu
                        ON rba.AgreementId = abu.AgreementId
                       AND abu.UserId = rbu.UserId 
                       AND abu.IsDeleted = 0
                    INNER JOIN Agreement ag
                        ON ag.AgreementId = abu.AgreementId
                       AND ag.IsDeleted = 0
                    WHERE rbu.UserId = @UserId
                      AND rbu.Active = 1";
            
            using var connection = CreateConnection();
            return await connection.QueryAsync<UserRoleAgreementDto>(sql, new { UserId = userId });
        }

        public async Task UpdateUserRoles(int userId, IEnumerable<long> roleIds, long updaterUserId)
        {
            var ids = (roleIds ?? []).Distinct().Where(r => r > 0).ToArray();
            
            using var connection = CreateConnection();
            connection.Open();
            using var tx = connection.BeginTransaction();

            try
            { 
                var deleteSql = @"UPDATE [dbo].[RoleByUser]
                                  SET IsDeleted = 1
                                  WHERE UserId = @UserId 
                                    AND IsDeleted = 0
                                    AND (@IdsCount = 0 OR RoleId NOT IN @Ids)";
                await connection.ExecuteAsync(deleteSql, new { UserId = userId, Ids = ids, IdsCount = ids.Length }, tx);
 
                var reactivateSql = @"UPDATE [dbo].[RoleByUser]
                                      SET IsDeleted = 0, Active = 1
                                      WHERE UserId = @UserId 
                                        AND RoleId IN @Ids 
                                        AND IsDeleted = 1";
                await connection.ExecuteAsync(reactivateSql, new { UserId = userId, Ids = ids }, tx);

         
                var insertSql = @"INSERT INTO [dbo].[RoleByUser] (RoleId, UserId, CreatorUserId, CreatedDate, Active, IsDeleted)
                                  SELECT @RoleId, @UserId, @CreatorUserId, GETDATE(), 1, 0
                                  WHERE NOT EXISTS (
                                      SELECT 1 FROM [dbo].[RoleByUser]
                                      WHERE UserId = @UserId AND RoleId = @RoleId
                                  );";
                
                foreach (var roleId in ids)
                {
                    await connection.ExecuteAsync(insertSql, new { 
                        RoleId = roleId, 
                        UserId = userId, 
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
