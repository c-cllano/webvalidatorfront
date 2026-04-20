using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class RolePermissionRepository(SQLServerConnectionFactory factory) : IRolePermissionRepository
    {
        private readonly SQLServerConnectionFactory _factory = factory;

        public async Task<IEnumerable<long>> GetPermissionIdsByRoleId(long roleId)
        {
            var sql = @"SELECT PermissionId FROM dbo.RolePermission WHERE RoleId = @RoleId AND Active = 1 AND IsDeleted = 0";
            using var conn = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await conn.QueryAsync<long>(sql, new { RoleId = roleId });
        }

        public async Task<List<Domain.Entities.RoleByUserNew>> GetRoleByUser(
      long userId,
      long? agreementId
  )
        {
            var sql = @"
        SELECT DISTINCT 
            RABY.UserId,
            ROL.RoleId,
            ROL.Name AS NameRole,
            AGR.Name AS NameAgreement,
            AGR.AgreementId
        FROM RoleAgreementByUser RABY
        INNER JOIN Role ROL 
            ON ROL.RoleId = RABY.RoleId
           AND ROL.Active = 1 
           AND ROL.IsDeleted = 0
        INNER JOIN Agreement AGR
        ON RABY.AgreementId = AGR.AgreementId
        WHERE RABY.UserId = @UserId
          AND RABY.Active = 1
          AND RABY.IsDeleted = 0
    ";

            if (agreementId.HasValue && agreementId.Value > 0)
            {
                sql += " AND RABY.AgreementId = @AgreementId";
            }

            using var conn = _factory.CreateConnection(ConnectionsName.OKeyConnection);

            var result = await conn.QueryAsync<Domain.Entities.RoleByUserNew>(
                sql,
                new
                {
                    UserId = userId,
                    AgreementId = agreementId
                }
            );

            return result.ToList();
        }

        public async Task UpsertRolePermissions(long roleId, IEnumerable<long> permissionIds, long userId)
        {
            var ids = (permissionIds ?? []).Distinct().ToArray();
            using var conn = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                var deleteSql = @"UPDATE dbo.RolePermission
                                   SET IsDeleted = 1, UpdatedDate = GETDATE()
                                   WHERE RoleId = @RoleId AND IsDeleted = 0
                                     AND (@IdsCount = 0 OR PermissionId NOT IN @Ids)";
                await conn.ExecuteAsync(deleteSql, new { RoleId = roleId, Ids = ids, IdsCount = ids.Length }, tx);

                var reactivateSql = @"UPDATE dbo.RolePermission
                                       SET IsDeleted = 0, UpdatedDate = GETDATE()
                                       WHERE RoleId = @RoleId AND PermissionId IN @Ids AND IsDeleted = 1";
                await conn.ExecuteAsync(reactivateSql, new { RoleId = roleId, Ids = ids }, tx);

                var insertSql = @"INSERT INTO dbo.RolePermission (RoleId, PermissionId, CreatedDate, CreatorUserId,
                                    Active, IsDeleted)
                                  SELECT @RoleId, @PermissionId, GETDATE(), @UserId, 1, 0
                                  WHERE NOT EXISTS (
                                      SELECT 1 FROM dbo.RolePermission 
                                      WHERE RoleId = @RoleId AND PermissionId = @PermissionId
                                  );";
                foreach (var pid in ids)
                {
                    await conn.ExecuteAsync(insertSql, new {
                        RoleId = roleId,
                        PermissionId = pid,
                        UserId = userId
                    }, tx);
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback(); 
            }
        }
    }
}
