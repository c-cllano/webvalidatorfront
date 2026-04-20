using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class PasswordRecoveryHistoryRepository(SQLServerConnectionFactory factory) : IPasswordRecoveryHistoryRepository
    {
        private readonly SQLServerConnectionFactory _factory = factory;

        public async Task<bool> GetByToken(string token, string email)
        {
            using var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            var query = @"SELECT TOP 1 Token FROM PasswordRecoveryHistory WHERE Token = @token AND Email = @email";
            var result = await connection.QueryFirstOrDefaultAsync(query, new { token, email });
            return result != null;
        }

        public async Task SaveToken(PasswordRecoveryHistory passwordRecoveryHistory)
        {
            using var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);

            var query = """
                INSERT INTO PasswordRecoveryHistory
                   (
                        Email, 
                        CreateDate, 
                        Token
                    )
                    VALUES 
                    (
                        @Email, 
                        @CreateDate, 
                        @Token
                    );
                """;

            await connection.ExecuteAsync(query, new
            {
                passwordRecoveryHistory.Email,
                passwordRecoveryHistory.CreateDate,
                passwordRecoveryHistory.Token
            });
        }
    }
}
