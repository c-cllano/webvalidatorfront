using Dapper; 
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class PasswordRecoveryNotificationRepository(SQLServerConnectionFactory factory) : IPasswordRecoveryNotificationRepository
    {
        private readonly SQLServerConnectionFactory _factory = factory;
        public async Task SaveAsync(PasswordRecoveryNotification passwordRecoveryNotification)
        {
            using var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);

            var query = """
                MERGE PasswordRecoveryNotifications AS target
                USING (SELECT @UserId AS UserId, @Email AS Email) AS source
                ON target.UserId = source.UserId AND target.Email = source.Email
                WHEN MATCHED THEN
                    UPDATE SET 
                        NotificationCount = target.NotificationCount + 1,
                        LastSentAt = @SentAt,
                        UpdatedAt = GETDATE()
                WHEN NOT MATCHED THEN
                    INSERT (UserId, Email, NotificationCount, LastSentAt, CreatedAt, UpdatedAt)
                    VALUES (@UserId, @Email, 1, @SentAt, GETDATE(), GETDATE());
                """;

            await connection.ExecuteAsync(query, new
            {
                passwordRecoveryNotification.UserId,
                passwordRecoveryNotification.Email,
                passwordRecoveryNotification.SentAt,
            });
        }

        public async Task SaveToken(PasswordRecoveryNotification passwordRecoveryNotification)
        {
            using var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            var query = @"UPDATE    PasswordRecoveryNotifications 
                          SET       Token = @Token
                          WHERE     Email = @Email";

            await connection.ExecuteAsync(query, new
            {
                passwordRecoveryNotification.Token,
                passwordRecoveryNotification.Email
            });
        }

        public async Task<bool> GetByToken(string token)
        {
            using var connection = _factory.CreateConnection(ConnectionsName.OKeyConnection);
            var query = @"SELECT Token FROM PasswordRecoveryNotifications WHERE Token = @token";
            var result = await connection.QueryFirstOrDefaultAsync(query, new { token });
            return result != null;
        }
    }
}
