using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Utilities;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ValidationProcessScoresRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IValidationProcessScoresRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task SaveValidationProcessScoresAsync(
            long validationProcessId,
            decimal scoreValue,
            ScoresCode scoreCode
        )
        {
            string scoreName = scoreCode.GetDescription();

            long? validationProcessScoresId = await GetValidationProcessScoresByValidationProcessIdAsync(
                validationProcessId
            );

            if (validationProcessScoresId == null)
            {
                await CreateEntityAsync(validationProcessId, scoreValue, scoreName);
            }
            else
            {
                await UpdateEntityAsync(validationProcessScoresId.Value, scoreValue, scoreName);
            }
        }

        private async Task<long?> GetValidationProcessScoresByValidationProcessIdAsync(long validationProcessId)
        {
            long? validationProcessScoresId = null!;

            string sql = @"
                SELECT TOP 1
                    ValidationProcessScoresId
                FROM [dbo].[ValidationProcessScores]
                WHERE ValidationProcessId = @validationProcessId
                ORDER BY CreatedDate DESC
            ";

            using var connection = CreateConnection();

            var entity = await connection.QueryFirstOrDefaultAsync<ValidationProcessScores>(sql, new { validationProcessId });

            if (entity != null)
                validationProcessScoresId = entity.ValidationProcessScoresId;

            return validationProcessScoresId;
        }

        private async Task<ValidationProcessScores> CreateEntityAsync(
            long validationProcessId,
            decimal scoreValue,
            string scoreName
        )
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[ValidationProcessScores](
                    [ValidationProcessId]
                   ,[Scores]
                   ,[CreatedDate]
                   ,[Active]
                   ,[IsDeleted]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @validationProcessId
                   ,JSON_MODIFY(
                        '{""Scores"":{}}',
                        CONCAT('$.Scores.', @scoreName),
                        @scoreValue
                    )
                   ,@CreatedDate
                   ,@Active
                   ,@IsDeleted
                )
            ";

            using var connection = CreateConnection();

            return await connection.QuerySingleAsync<ValidationProcessScores>(
                sqlCommand,
                new
                {
                    validationProcessId,
                    scoreName,
                    scoreValue,
                    CreatedDate = DateTime.UtcNow.AddHours(-5),
                    Active = 1,
                    IsDeleted = 0
                }
            );
        }

        private async Task<ValidationProcessScores> UpdateEntityAsync(
            long validationProcessScoresId,
            decimal scoreValue,
            string scoreName
        )
        {
            string sqlCommand = @"
                UPDATE [dbo].[ValidationProcessScores]
                SET 
                    [Scores] = 
                        CASE
                            WHEN [Scores] IS NULL OR LTRIM(RTRIM([Scores])) = ''
                            THEN
                                JSON_MODIFY(
                                    '{""Scores"":{}}',
                                    CONCAT('$.Scores.', @scoreName),
                                    @scoreValue
                                )
                            WHEN ISJSON([Scores]) = 1
                            THEN
                                JSON_MODIFY(
                                    [Scores],
                                    CONCAT('$.Scores.', @scoreName),
                                    @scoreValue
                                )
                            ELSE
                                JSON_MODIFY(
                                    '{""Scores"":{}}',
                                    CONCAT('$.Scores.', @scoreName),
                                    @scoreValue
                                )
                        END,
                    [UpdatedDate] = @UpdatedDate
                OUTPUT INSERTED.*
                WHERE ValidationProcessScoresId = @validationProcessScoresId
            ";

            using var connection = CreateConnection();

            return await connection.QuerySingleAsync<ValidationProcessScores>(
                sqlCommand,
                new
                {
                    scoreName,
                    scoreValue,
                    UpdatedDate = DateTime.UtcNow.AddHours(-5),
                    validationProcessScoresId
                }
            );
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
