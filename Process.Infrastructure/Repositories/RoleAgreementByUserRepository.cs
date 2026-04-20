using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{

    public class RoleAgreementByUserRepository(SQLServerConnectionFactory connectionFactory) : IRoleAgreementByUserRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<long> AddAsync(RoleAgreementByUser roleByUser)
        {
            var sql = @"INSERT INTO [dbo].[RoleAgreementByUser] (RoleId, UserId, AgreementId, CreatorUserId, CreatedDate, Active, IsDeleted)
                        VALUES (@RoleId, @UserId,  @AgreementId, @CreatorUserId, @CreatedDate, @Active, @IsDeleted);
                        SELECT CAST(SCOPE_IDENTITY() as bigint);";

            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(sql, roleByUser);
        }

        public async Task<IEnumerable<RoleAgreementByUser?>> GetByUserRoleAndAgreemenmt(int userId, long roleId, int agreementId)
        {
            var sql = @"SELECT [RoleAgreementByUserId]
                          ,[UserId]
                          ,[RoleId]
                          ,[AgreementId]
                          ,[CreatorUserId]
                          ,[CreatedDate]
                          ,[UpdatedDate]
                          ,[Active]
                          ,[IsDeleted]
              FROM [dbo].[RoleAgreementByUser]
              where userId =@userId and roleId= @roleId and AgreementId = @agreementId and IsDeleted =1 and Active =1
        ";

            using var connection = CreateConnection();
            return await connection.QueryAsync<RoleAgreementByUser>(sql, new { UserId= userId,  RoleId= roleId,AgreementId = agreementId });
        }



        public async Task<IEnumerable<UserRoleAgreementNewDto>> GetUserRolesAndAgreementsAsync(int userId)
        {
            var sql = @"       
                  SELECT DISTINCT
                          rbu.RoleAgreementByUserId,
                          r.RoleId,
                          r.Name AS RoleName,
                          ag.Name AS AgreementName,
                          ag.AgreementId
                      FROM RoleAgreementByUser rbu
                      INNER JOIN Role r 
                          ON rbu.RoleId = r.RoleId 
                         AND r.IsDeleted = 0 and rbu.IsDeleted = 0
                      INNER JOIN Agreement ag
                          ON ag.AgreementId = rbu.AgreementId
                         AND ag.IsDeleted = 0
                      WHERE rbu.UserId = @userId
                        AND rbu.Active = 1";

            using var connection = CreateConnection();
            return await connection.QueryAsync<UserRoleAgreementNewDto>(sql, new { UserId = userId });
        }


        /**NUEVOS METODOS ROLES Y CONVENIOS**/

        public async Task<IEnumerable<RoleAgreementByUser>> getOldRolAgreementByUserId(int userId)
        {
            var sql = @"
        SELECT * 
        FROM RoleAgreementByUser 
        WHERE
           UserId = @UserId";

            using var connection = CreateConnection();
            return await connection.QueryAsync<RoleAgreementByUser>(sql, new { UserId = userId });
        }

        public async Task RemoveRoleAgreement(object item)
        {
            var roleAgreement = (RoleAgreementByUser)item;

            var sql = @"
        UPDATE RoleAgreementByUser 
        SET Active = 0, IsDeleted = 1 
        WHERE RoleAgreementByUserId = @RoleAgreementByUserId";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                roleAgreement.RoleAgreementByUserId
            });
        }

        public async Task UpdateUserRoles(int userId, IEnumerable<long> roleIds, long updaterUserId)
        {
            var sql = @"
        UPDATE RoleAgreementByUser 
        SET Active = 0, IsDeleted = 1 
        WHERE userId = @userId";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                userId
            });
        }

        public async Task UpdateRoleAgreement(object item)
        {
            var roleAgreement = (RoleAgreementByUser)item;


            var sql = @"
        UPDATE RoleAgreementByUser 
        SET Active = 1, IsDeleted = 0 
        WHERE RoleAgreementByUserId = @RoleAgreementByUserId";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                roleAgreement.RoleAgreementByUserId
            });
        }


        public async Task AddRoleAgreement(RoleAgreementByUser roleAgreementByUser)
        {
            var sql = @"
        INSERT INTO RoleAgreementByUser (UserId, RoleId, AgreementId, CreatorUserId, CreatedDate , Active, IsDeleted)
        VALUES (@UserId, @RoleId, @AgreementId, @CreatorUserId, @CreatedDate , @Active, @IsDeleted);
        ";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                roleAgreementByUser.UserId,
                roleAgreementByUser.RoleId,
                roleAgreementByUser.AgreementId,
                roleAgreementByUser.CreatorUserId,
                roleAgreementByUser.CreatedDate,
                roleAgreementByUser.Active,
                roleAgreementByUser.IsDeleted
            });
        }


        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }

    }
}
