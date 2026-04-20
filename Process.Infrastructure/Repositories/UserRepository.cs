using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;

namespace Process.Infrastructure.Repositories
{
    public class UserRepository(SQLServerConnectionFactory connectionFactory) : IUserRepository
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();
        public async Task<UserInfo?> GetUserById(long id)
        {
            string sql = "SELECT * FROM transaccional.Usuario WHERE UsuarioId = @Id";
            return await _connection.QueryFirstOrDefaultAsync<UserInfo>(sql, new { Id = id });
        }

    }
}