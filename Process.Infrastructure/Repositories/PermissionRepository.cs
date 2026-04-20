using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class PermissionRepository(SQLServerConnectionFactory factory) : IPermissionRepository
    {
        private readonly SQLServerConnectionFactory _factory = factory;

        public async Task<IEnumerable<Permission>> GetPermissionsByMenuIds(IEnumerable<long> menuIds)
        {
            var ids = (menuIds ?? []).ToArray();
            var sql = @"SELECT PermissionId, MenuId, Code, Name, [Order], Active, IsDeleted, CreatedDate, UpdatedDate
                        FROM dbo.Permission
                        WHERE (@IdsCount = 0 OR MenuId IN @Ids) AND Active = 1 AND IsDeleted = 0
                        ORDER BY [Order]";
            using var conn = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await conn.QueryAsync<Permission>(sql, new { Ids = ids, IdsCount = ids.Length });
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsWithMenu()
        {
            var sql = @"SELECT PermissionId, MenuId, Code, Name, [Order], Active, IsDeleted, CreatedDate, UpdatedDate
                        FROM dbo.Permission
                        WHERE Active = 1 AND IsDeleted = 0
                        ORDER BY MenuId, [Order]";
            using var conn = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await conn.QueryAsync<Permission>(sql);
        }
    }
}
