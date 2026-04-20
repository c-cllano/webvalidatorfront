using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class RoleRepository(SQLServerConnectionFactory connectionFactory) : IRoleRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;
        public async Task<int> DeleteRole(long roleId)
        {
            using var connection = CreateConnection();
            connection.Open();
            using var tx = connection.BeginTransaction();

            try
            {
                var sql = @"
        UPDATE [dbo].[Role]
        SET IsDeleted = 1,
            UpdatedDate = GETDATE()
        WHERE RoleId = @RoleId
        AND IsDeleted = 0;

        UPDATE [dbo].[RoleMenu]
        SET IsDeleted = 1,
            UpdatedDate = GETDATE()
        WHERE RoleId = @RoleId
        AND IsDeleted = 0;
        ";

                var result = await connection.ExecuteAsync(sql, new { RoleId = roleId }, tx);

                tx.Commit();
                return result;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }


        public async Task<IEnumerable<RoleDto>> GetAllRoles(Guid clientGuid, string[]? rolName, string? status, int? pageNumber, int? pageSize)
        {
            var sql = @"SELECT 
                            R.RoleId AS Id,
                            R.Name,
                            CASE WHEN R.Active = 1 THEN 'Activo' ELSE 'Inactivo' END AS Status,
                            COUNT(distinct UR.UserId) AS Users,
                            R.ClientId
                        FROM [dbo].[Role] R
                        INNER JOIN dbo.Client c ON c.ClientId = R.ClientId  AND c.IsDeleted = 0
                        LEFT JOIN[dbo].[RoleAgreementByUser] UR ON R.RoleId = UR.RoleId  AND UR.IsDeleted = 0
                        LEFT JOIN [dbo].[User] U ON UR.UserId = U.UserId AND U.IsDeleted = 0
                        WHERE c.ClientToken = @clientGuid
                          AND R.IsDeleted = 0
                          AND (@Rol IS NULL OR R.Name LIKE '%' + @Rol + '%')
                          AND (@Estado IS NULL OR R.Active = @Estado)
                        GROUP BY R.RoleId, R.Name, R.Active, R.ClientId
                        ORDER BY R.RoleId";

             

            using var connection = CreateConnection();
            var result = await connection.QueryAsync<RoleDto>(sql, new
            {
                clientGuid,
                Rol = rolName != null && rolName.Length > 0 ? string.Join(",", rolName) : null,
                Estado = string.IsNullOrEmpty(status) ? (bool?)null : status.Equals("activo", StringComparison.CurrentCultureIgnoreCase)                
            });
            return result;
        }
         

        public async Task<Role?> GetRoleById(long roleId)
        {
            var sql = "SELECT * FROM [dbo].[Role] WHERE RoleId = @roleId  ";
            using var connection = CreateConnection();
            var result = await connection.QuerySingleOrDefaultAsync<Role>(sql, new { roleId });
            return result;

        }

        public async Task<Role?> GetRoleByName(string name, long clientId)
        { 
            var sql = @"
            SELECT * 
            FROM [dbo].[Role] 
            WHERE ClientId = @clientId  
            AND LOWER(LTRIM(RTRIM(Name))) = @normalizedName 
            AND IsDeleted = 0";

            var normalizedName = name.Trim().ToLower();
            
            using var connection = CreateConnection();
            var result = await connection.QuerySingleOrDefaultAsync<Role>(sql, new { clientId, normalizedName });
            return result;
        }

        public async Task<int> InsertRole(Role role)
        {
            var sql = @"INSERT INTO [dbo].[Role] (Name, ClientId, CreatedDate, CreatorUserId, Active, IsDeleted)
                        VALUES (@Name, @ClientId, @CreatedDate, @CreatorUserId, @Active, 0);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            using var connection = CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(sql, new
            {
                role.Name,
                role.ClientId,
                role.CreatedDate,
                role.CreatorUserId,
                role.Active
            });
            return result;

        }

        public async Task<int> UpdateRole(Role role)
        {
          
            var sql = @"UPDATE [dbo].[Role]
                        SET Name = @Name,
                            UpdatedDate = @UpdatedDate,
                            Active = @Active
                        WHERE RoleId = @RoleId AND IsDeleted = 0";
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(sql, new
            {
                role.Name,
                role.UpdatedDate,
                role.Active,
                role.RoleId
            });
            return result;

        }


        public async Task<int> GetUserCountByRoleId(long roleId)
        {
            var sql = @"SELECT COUNT(DISTINCT UR.UserId) 
                        FROM [dbo].[RoleAgreementByUser] UR
                        INNER JOIN [dbo].[User] U ON UR.UserId = U.UserId AND U.IsDeleted = 0
                        AND UR.IsDeleted = 0
                        WHERE UR.RoleId = @RoleId";
            using var connection = CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(sql, new { RoleId = roleId });
            return result;
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
