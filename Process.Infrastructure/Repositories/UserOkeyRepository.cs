using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class UserOkeyRepository(SQLServerConnectionFactory connectionFactory) : IUserOkeyRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<long> AddAsync(UserOkey user)
        {
            var sql = @"INSERT INTO [User] (UserId, UserGuid, CellPhone, CreatorUserId, CreatedDate, UpdatedDate, Active, IsDeleted)
                        VALUES (@UserId, @UserGuid, @CellPhone, @CreatorUserId, @CreatedDate, @UpdatedDate, @Active, @IsDeleted);
                        SELECT @UserId;";
            
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(sql, user);
        }

        public async Task<UserOkey?> GetByUserIdAsync(long userId)
        {
            var sql = @"SELECT UserId, UserGuid, CellPhone, CreatorUserId, CreatedDate, UpdatedDate, Active, IsDeleted
                        FROM [User]
                        WHERE UserId = @UserId AND IsDeleted = 0";
            
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserOkey>(sql, new { UserId = userId });
        }

        public async Task<List<UserOkey>> GetUsers(IEnumerable<int>? usersId)
        {
            if (usersId == null || !usersId.Any())
                return new List<UserOkey>();

            var sql = @"SELECT UserId, UserGuid, CellPhone, CreatorUserId, CreatedDate, UpdatedDate, Active, IsDeleted
                        FROM [User]
                        WHERE UserId IN @UserIds AND IsDeleted = 0";

            using var connection = CreateConnection();
            var result = await connection.QueryAsync<UserOkey>(sql, new { UserIds = usersId });
            return [.. result];
        }

        public async Task UpdateAsync(UserOkey user)
        {
            var sql = @"UPDATE [User]
                        SET CellPhone = @CellPhone,
                            UpdatedDate = @UpdatedDate,
                            Active = @Active,
                            IsDeleted = @IsDeleted
                        WHERE UserId = @UserId";
            
            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, user);
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
