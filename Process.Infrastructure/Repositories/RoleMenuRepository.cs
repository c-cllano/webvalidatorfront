using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class RoleMenuRepository(SQLServerConnectionFactory connectionFactory) : IRoleMenuRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
         
        public async Task<int> UpdateRoleMenu(RoleMenu roleMenu)
        {
            var sql = @"UPDATE [dbo].[RoleMenu]
                        SET MenuId = @MenuId,
                            UpdatedDate = @UpdatedDate,
                            Active = @Active
                        WHERE RoleMenuId = @RoleMenuId AND IsDeleted = 0";
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(sql, new
            {
                roleMenu.MenuId,
                roleMenu.UpdatedDate,
                roleMenu.Active,
                roleMenu.RoleMenuId
            });
            return result;
        }

        public async Task<int> DeleteRoleMenu(long roleMenuId)
        {
            var sql = @"UPDATE [dbo].[RoleMenu]
                        SET IsDeleted = 1,
                            UpdatedDate = GETDATE()
                        WHERE RoleMenuId = @RoleMenuId AND IsDeleted = 0";
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { RoleMenuId = roleMenuId });
            return result;
        }

        public async Task<int> InsertRoleMenus(long roleId, IEnumerable<long> menuIds, long creatorUserId)
        {
            if (menuIds == null) return 0;
            var ids = menuIds.Distinct().ToArray();
            if (ids.Length == 0) return 0;

            using var connection = CreateConnection();
            connection.Open();
            using var tx = connection.BeginTransaction();
            try
            {
                //Obtener todos los hijos (todos los niveles)
                var cteSql = @"
            WITH MenuCTE AS
            (
                SELECT MenuId, ParentId
                FROM Menu
                WHERE MenuId IN @RootIds
                AND Active = 1 AND IsDeleted = 0

                UNION ALL

                SELECT m.MenuId, m.ParentId
                FROM Menu m
                INNER JOIN MenuCTE c ON m.ParentId = c.MenuId
                WHERE m.Active = 1 AND m.IsDeleted = 0
            )
            SELECT DISTINCT MenuId FROM MenuCTE;
        ";

                var allMenuIds = (await connection.QueryAsync<long>(
                    cteSql,
                    new { RootIds = ids },
                    tx
                )).ToArray();

                // Insertar evitando duplicados
                var insertSql = @"
            INSERT INTO [dbo].[RoleMenu] 
            ([RoleId], [MenuId], [CreatedDate], [CreatorUserId], [Active], [IsDeleted])
            SELECT @RoleId, @MenuId, GETDATE(), @CreatorUserId, 1, 0
            WHERE NOT EXISTS (
                SELECT 1 FROM [dbo].[RoleMenu] 
                WHERE RoleId = @RoleId AND MenuId = @MenuId AND IsDeleted = 0
            );
        ";

                var total = 0;

                foreach (var menuId in allMenuIds)
                {
                    total += await connection.ExecuteAsync(insertSql, new
                    {
                        RoleId = roleId,
                        MenuId = menuId,
                        CreatorUserId = creatorUserId
                    }, tx);
                }

                tx.Commit();
                return total;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }




        public async Task UpdateRoleMenus(long roleId, IEnumerable<long> menuIds, long updaterUserId)
        {
            var ids = (menuIds ?? []).Distinct().ToArray();

            using var connection = CreateConnection();
            connection.Open();
            using var tx = connection.BeginTransaction();

            try
            {
                //Expandir a todos los niveles (padres incluidos)
                var cteSql = @"
            WITH MenuCTE AS
            (
                SELECT MenuId, ParentId
                FROM Menu
                WHERE MenuId IN @RootIds
                AND Active = 1 AND IsDeleted = 0

                UNION ALL

                SELECT m.MenuId, m.ParentId
                FROM Menu m
                INNER JOIN MenuCTE c ON m.ParentId = c.MenuId
                WHERE m.Active = 1 AND m.IsDeleted = 0
            )
            SELECT DISTINCT MenuId FROM MenuCTE;
        ";

                var expandedIds = ids.Length == 0
                    ? []
                    : (await connection.QueryAsync<long>(
                        cteSql,
                        new { RootIds = ids },
                        tx
                    )).ToArray();

                // delete lo que ya no debe estar
                var deleteSql = @"
            UPDATE [dbo].[RoleMenu]
            SET Active= 0, IsDeleted = 1, UpdatedDate = GETDATE()
            WHERE RoleId = @RoleId 
            AND IsDeleted = 0
            AND (@IdsCount = 0 OR MenuId NOT IN @Ids);
        ";

                await connection.ExecuteAsync(deleteSql, new
                {
                    RoleId = roleId,
                    Ids = expandedIds,
                    IdsCount = expandedIds.Length
                }, tx);

                //Reactivar los que estaban borrados
                if (expandedIds.Length > 0)
                {
                    var reactivateSql = @"
                UPDATE [dbo].[RoleMenu]
                SET Active=1,  IsDeleted = 0, UpdatedDate = GETDATE()
                WHERE RoleId = @RoleId 
                AND MenuId IN @Ids 
                AND IsDeleted = 1;
            ";

                    await connection.ExecuteAsync(reactivateSql, new
                    {
                        RoleId = roleId,
                        Ids = expandedIds
                    }, tx);
                }

                //Insertar los que no existen
                var insertSql = @"
            INSERT INTO [dbo].[RoleMenu] 
            ([RoleId], [MenuId], [CreatedDate], [CreatorUserId], [Active], [IsDeleted])
            SELECT @RoleId, @MenuId, GETDATE(), @CreatorUserId, 1, 0
            WHERE NOT EXISTS (
                SELECT 1 FROM [dbo].[RoleMenu]
                WHERE RoleId = @RoleId AND MenuId = @MenuId
            );
        ";

                foreach (var menuId in expandedIds)
                {
                    await connection.ExecuteAsync(insertSql, new
                    {
                        RoleId = roleId,
                        MenuId = menuId,
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


        public async Task<IEnumerable<long>> GetMenuIdsByRoleId(long roleId)
        {
            var sql = @"SELECT MenuId
                        FROM [dbo].[RoleMenu]
                        WHERE RoleId = @RoleId AND IsDeleted = 0 AND Active = 1 ";
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<long>(sql, new { RoleId = roleId });
            return result;
        }
    }
}
    