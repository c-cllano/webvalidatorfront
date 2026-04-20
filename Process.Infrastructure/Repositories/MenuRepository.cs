using Dapper;
using Process.Application.Menus;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class MenuRepository(SQLServerConnectionFactory factory) : IMenuRepository
    {
        private readonly SQLServerConnectionFactory _factory = factory;
        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            var sql = @"
                        SELECT 
                            m.MenuId,
                            m.ParentId,
                            m.[Order],
                            m.Title,
                            m.[Description],
                            m.Icon,
                            m.Link,
                            m.Selected,
                            m.Active
                        FROM dbo.Menu m
                        WHERE m.Active = 1 AND m.IsDeleted = 0
                        ORDER BY m.[Order];";
            var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await connection.QueryAsync<Menu>(sql); 
        }

        public async Task<IEnumerable<Menu>> GetMenusByRoleAsync(long roleId)
        {
            var sql = @"
                    SELECT 
                        m.MenuId,
                        m.ParentId,
                        m.[Order],
                        m.Title,
                        m.[Description],
                        m.Icon,
                        m.Link,
                        m.Selected,
                        m.Active
                    FROM dbo.Menu m
                    INNER JOIN dbo.RoleMenu rm ON m.MenuId = rm.MenuId
                    WHERE rm.RoleId = @RoleId
                      AND m.Active = 1 AND m.IsDeleted = 0
                      AND rm.Active = 1 AND rm.IsDeleted = 0
                    ORDER BY m.[Order];";

            var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await connection.QueryAsync<Menu>(sql, new { RoleId = roleId });
        }

        public async Task<IEnumerable<string>> GetLeafMenuNamesByRoleAsync(long roleId)
        {
            var sql = @"
                    WITH RoleMenus AS (
                        SELECT 
                            m.MenuId,
                            m.ParentId,
                            m.[Order],
                            m.Title
                        FROM dbo.Menu m
                        INNER JOIN dbo.RoleMenu rm ON m.MenuId = rm.MenuId
                        WHERE rm.RoleId = @RoleId
                          AND m.Active = 1 AND m.IsDeleted =  0 AND m.Visible=1
                          AND rm.Active = 1 AND rm.IsDeleted = 0
                    ),
                    ParentsWithChildren AS (
                        SELECT DISTINCT rm1.ParentId
                        FROM RoleMenus rm1
                        INNER JOIN RoleMenus rm2 ON rm1.ParentId = rm2.MenuId
                        WHERE rm1.ParentId IS NOT NULL
                    )
                    SELECT DISTINCT rm.Title
                    FROM RoleMenus rm
                    LEFT JOIN ParentsWithChildren pwc ON rm.MenuId = pwc.ParentId
                    ORDER BY rm.Title;";

            var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await connection.QueryAsync<string>(sql, new { RoleId = roleId });
        }

        public async Task<IEnumerable<Menu>> GetMenusByAgreementAndUserAsync(long agreementId, long userId)
        {
            var sql = @"
                                          SELECT 
                            m.MenuId,
                            m.ParentId,
                            m.[Order],
                            m.Title,
                            m.[Description],
                            m.Icon,
                            m.Link,
                            m.Selected,
                            m.Active,
                            m.visible
                        FROM Menu m
                        WHERE m.Active = 1 AND m.IsDeleted = 0
                        and MenuId in (
                                               select MenuId from RoleMenu where roleId 
                        IN ( select RoleId from [Role] where Active = 1 AND IsDeleted = 0 and RoleId in ( select RoleId from RoleAgreementByUser 
                        where 
                        AgreementId in (@AgreementId) and
                        UserId in ( @UserId )
                        and Active = 1 and IsDeleted = 0 ))
                        and  RoleMenu.Active = 1 and RoleMenu.IsDeleted = 0
                        )
                        ORDER BY m.[Order]
";

            var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await connection.QueryAsync<Menu>(sql, new { AgreementId = agreementId, UserId = userId });
        }


        public async Task<List<Domain.Entities.RoleActual>> GetActualRoleBySideMenu(long userId, long agreementId)
        {
                        var sql = @"
                                         select raby.RoleAgreementByUserId,
            RABY.UserId,
            RABY.RoleId,
            rol.Name as NameRole,
            RABY.AgreementId,
            AGR.Name as NameAgreement

            from RoleAgreementByUser  RABY

            inner join role ROL on (rol.RoleId = RABY.RoleId)

            inner join Agreement AGR on ( AGR.AgreementId = RABY.AgreementId)

            where RABY.UserId= @UserId and RABY.AgreementId = @AgreementId
            and RABY.active = 1 and RABY.isdeleted = 0
            and rol.Active =1 and rol.IsDeleted =0
            and AGR.Active = 1 and AGR.IsDeleted = 0
            ";

            var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            var result = await connection.QueryAsync<RoleActual>(sql, new { AgreementId = agreementId, UserId = userId });
            return result.ToList();

        }

        public async Task<IEnumerable<MenuWithPermissionsDto>> GetMenusWithPermissionsAsync(long? roleId)
        {
            var sql = @"
                SELECT 
                    m.MenuId,
                    m.ParentId,
                    m.[Order],
                    m.Title,
                    m.[Description],
                    m.Icon,
                    m.Link,
                    m.Active,
                    m.Visible,
                    CASE WHEN rm.RoleMenuId IS NOT NULL THEN 1 ELSE 0 END AS MenuSelected,
                    p.PermissionId,
                    p.Code AS PermissionCode,
                    p.Name AS PermissionName,
                    p.[Order] AS PermissionOrder,
                    CASE WHEN rp.RolePermissionId IS NOT NULL THEN 1 ELSE 0 END AS PermissionSelected
                FROM dbo.Menu m
                LEFT JOIN dbo.Permission p ON m.MenuId = p.MenuId 
                    AND p.Active = 1 AND p.IsDeleted = 0
                LEFT JOIN dbo.RoleMenu rm ON m.MenuId = rm.MenuId 
                    AND rm.RoleId = @RoleId 
                    AND rm.Active = 1 AND rm.IsDeleted = 0
                LEFT JOIN dbo.RolePermission rp ON p.PermissionId = rp.PermissionId 
                    AND rp.RoleId = @RoleId 
                    AND rp.Active = 1 AND rp.IsDeleted = 0
                WHERE m.Active = 1 AND m.IsDeleted = 0 AND m.Visible = 1
                ORDER BY m.[Order], p.[Order];";

            var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            return await connection.QueryAsync<MenuWithPermissionsDto>(sql, new { RoleId = roleId });
        }
    }
}
