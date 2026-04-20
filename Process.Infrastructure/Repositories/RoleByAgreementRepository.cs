using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class RoleByAgreementRepository(SQLServerConnectionFactory connectionFactory) : IRoleByAgreementRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<long> AddAsync(RoleByAgreement roleByAgreement)
        {
            var sql = @"INSERT INTO [dbo].[RoleByAgreement] 
                        (RoleId, AgreementId, CreatorUserId, CreatedDate, Active, IsDeleted)
                        VALUES (@RoleId, @AgreementId, @CreatorUserId, @CreatedDate, @Active, @IsDeleted);
                        SELECT CAST(SCOPE_IDENTITY() as bigint);";
            
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(sql, roleByAgreement);
        }

        public async Task<RoleByAgreement?> GetByIdAsync(long roleByAgreementId)
        {
            var sql = @"SELECT 
                            rba.RoleByAgreementId,
                            rba.RoleId,
                            rba.AgreementId,
                            rba.CreatorUserId,
                            rba.CreatedDate,
                            rba.UpdatedDate,
                            rba.Active,
                            rba.IsDeleted,
                            r.Name AS RoleName,
                            a.Name AS AgreementName
                        FROM [dbo].[RoleByAgreement] rba
                        INNER JOIN [dbo].[Role] r ON rba.RoleId = r.RoleId
                        INNER JOIN [dbo].[Agreement] a ON rba.AgreementId = a.AgreementId
                        WHERE rba.RoleByAgreementId = @RoleByAgreementId 
                            AND rba.IsDeleted = 0";
            
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<RoleByAgreement>(sql, 
                new { RoleByAgreementId = roleByAgreementId });
        }

        public async Task<IEnumerable<RoleByAgreement>> GetByRoleIdAsync(long roleId)
        {
            var sql = @"SELECT 
                            rba.RoleByAgreementId,
                            rba.RoleId,
                            rba.AgreementId,
                            rba.CreatorUserId,
                            rba.CreatedDate,
                            rba.UpdatedDate,
                            rba.Active,
                            rba.IsDeleted,
                            r.Name AS RoleName,
                            a.Name AS AgreementName
                        FROM [dbo].[RoleByAgreement] rba
                        INNER JOIN [dbo].[Role] r ON rba.RoleId = r.RoleId
                        INNER JOIN [dbo].[Agreement] a ON rba.AgreementId = a.AgreementId
                        WHERE rba.RoleId = @RoleId 
                            AND rba.IsDeleted = 0
                        ORDER BY a.Name";
            
            using var connection = CreateConnection();
            return await connection.QueryAsync<RoleByAgreement>(sql, new { RoleId = roleId });
        }

        public async Task<IEnumerable<RoleByAgreement>> GetByAgreementIdAsync(int agreementId)
        {
            var sql = @"SELECT 
                            rba.RoleByAgreementId,
                            rba.RoleId,
                            rba.AgreementId,
                            rba.CreatorUserId,
                            rba.CreatedDate,
                            rba.UpdatedDate,
                            rba.Active,
                            rba.IsDeleted,
                            r.Name AS RoleName,
                            a.Name AS AgreementName
                        FROM [dbo].[RoleByAgreement] rba
                        INNER JOIN [dbo].[Role] r ON rba.RoleId = r.RoleId
                        INNER JOIN [dbo].[Agreement] a ON rba.AgreementId = a.AgreementId
                        WHERE rba.AgreementId = @AgreementId 
                            AND rba.IsDeleted = 0
                        ORDER BY r.Name";
            
            using var connection = CreateConnection();
            return await connection.QueryAsync<RoleByAgreement>(sql, new { AgreementId = agreementId });
        }

        public async Task<IEnumerable<RoleByAgreement>> GetAllAsync()
        {
            var sql = @"SELECT 
                            rba.RoleByAgreementId,
                            rba.RoleId,
                            rba.AgreementId,
                            rba.CreatorUserId,
                            rba.CreatedDate,
                            rba.UpdatedDate,
                            rba.Active,
                            rba.IsDeleted,
                            r.Name AS RoleName,
                            a.Name AS AgreementName
                        FROM [dbo].[RoleByAgreement] rba
                        INNER JOIN [dbo].[Role] r ON rba.RoleId = r.RoleId
                        INNER JOIN [dbo].[Agreement] a ON rba.AgreementId = a.AgreementId
                        WHERE rba.IsDeleted = 0
                        ORDER BY r.Name, a.Name";
            
            using var connection = CreateConnection();
            return await connection.QueryAsync<RoleByAgreement>(sql);
        }

        public async Task UpdateAsync(RoleByAgreement roleByAgreement)
        {
            var sql = @"UPDATE [dbo].[RoleByAgreement]
                        SET Active = @Active,
                            UpdatedDate = @UpdatedDate
                        WHERE RoleByAgreementId = @RoleByAgreementId";
            
            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, roleByAgreement);
        }

        public async Task DeleteAsync(long roleByAgreementId)
        {
            var sql = @"UPDATE [dbo].[RoleByAgreement]
                        SET IsDeleted = 1,
                            UpdatedDate = GETDATE()
                        WHERE RoleByAgreementId = @RoleByAgreementId";
            
            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, new { RoleByAgreementId = roleByAgreementId });
        }

        public async Task<bool> ExistsAsync(long roleId, int agreementId)
        {
            var sql = @"SELECT COUNT(1)
                        FROM [dbo].[RoleByAgreement]
                        WHERE RoleId = @RoleId 
                            AND AgreementId = @AgreementId 
                            AND IsDeleted = 0";
            
            using var connection = CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(sql, new { RoleId = roleId, AgreementId = agreementId });
            return count > 0;
        }

        public async Task UpdateRoleAgreements(long roleId, IEnumerable<int> agreementIds, long updaterUserId)
        {
            var ids = (agreementIds ?? []).Distinct().Where(a => a > 0).ToArray();
            
            using var connection = CreateConnection();
            connection.Open();
            using var tx = connection.BeginTransaction();

            try
            {
                // Eliminar agreements que ya no están en la lista
                var deleteSql = @"UPDATE [dbo].[RoleByAgreement]
                                  SET IsDeleted = 1, UpdatedDate = GETDATE()
                                  WHERE RoleId = @RoleId 
                                    AND IsDeleted = 0
                                    AND (@IdsCount = 0 OR AgreementId NOT IN @Ids)";
                await connection.ExecuteAsync(deleteSql, new { RoleId = roleId, Ids = ids, IdsCount = ids.Length }, tx);

                // Reactivar agreements que fueron eliminados
                var reactivateSql = @"UPDATE [dbo].[RoleByAgreement]
                                      SET IsDeleted = 0, Active = 1, UpdatedDate = GETDATE()
                                      WHERE RoleId = @RoleId 
                                        AND AgreementId IN @Ids 
                                        AND IsDeleted = 1";
                await connection.ExecuteAsync(reactivateSql, new { RoleId = roleId, Ids = ids }, tx);

                // Insertar nuevos agreements
                var insertSql = @"INSERT INTO [dbo].[RoleByAgreement] 
                                  (RoleId, AgreementId, CreatorUserId, CreatedDate, Active, IsDeleted)
                                  SELECT @RoleId, @AgreementId, @CreatorUserId, GETDATE(), 1, 0
                                  WHERE NOT EXISTS (
                                      SELECT 1 FROM [dbo].[RoleByAgreement]
                                      WHERE RoleId = @RoleId AND AgreementId = @AgreementId
                                  );";
                
                foreach (var agreementId in ids)
                {
                    await connection.ExecuteAsync(insertSql, new { 
                        RoleId = roleId, 
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
