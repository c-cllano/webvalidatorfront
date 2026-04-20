using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ForensicReviewProcessRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IForensicReviewProcessRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<ForensicReviewProcess?> GetForensicReviewProcessByValidationProcessIdAsync(long validationProcessId)
        {
            string sql = @"
                SELECT 
                    [ForensicReviewProcessId]
                    ,[ValidationProcessId]
                    ,[TxGuidForense]
                    ,[StatusForensicId]
                    ,[VerificationDate]
                    ,[Approved]
                    ,[Score]
                    ,[MainReason]
                    ,[OptionalReason]
                    ,[Description]
                    ,[Observation]
                    ,[CreatedDate]
                    ,[UpdatedDate]
                    ,[Active]
                    ,[IsDeleted]
                  FROM [dbo].[ForensicReviewProcess]
                  WHERE ValidationProcessId = @validationProcessId
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ForensicReviewProcess>(sql, new { validationProcessId });
        }

        public async Task<ForensicReviewProcess> SaveForensicReviewProcessAsync(ForensicReviewProcess forensicReviewProcess)
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[ForensicReviewProcess](
                    [ValidationProcessId],[TxGuidForense],[StatusForensicId],[VerificationDate],[Approved],[Score],[MainReason],
                    [OptionalReason],[Description],[Observation],[CreatedDate],[UpdatedDate],[Active],[IsDeleted]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @ValidationProcessId,@TxGuidForense,@StatusForensicId,@VerificationDate,@Approved,@Score,@MainReason,
                    @OptionalReason,@Description,@Observation,@CreatedDate,@UpdatedDate,@Active,@IsDeleted
                )
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<ForensicReviewProcess>(sqlCommand, forensicReviewProcess);
        }

        public async Task<ForensicReviewProcess> UpdateForensicReviewProcessAsync(ForensicReviewProcess forensicReviewProcess)
        {
            string sqlCommand = @"
                UPDATE [dbo].[ForensicReviewProcess]
                SET
                    [ValidationProcessId] = @ValidationProcessId,
                    [TxGuidForense] = @TxGuidForense,
                    [StatusForensicId] = @StatusForensicId,
                    [VerificationDate] = @VerificationDate,
                    [Approved] = @Approved,
                    [Score] = @Score,
                    [MainReason] = @MainReason,
                    [OptionalReason] = @OptionalReason,
                    [Description] = @Description,
                    [Observation] = @Observation,
                    [CreatedDate] = @CreatedDate,
                    [UpdatedDate] = @UpdatedDate,
                    [Active] = @Active,
                    [IsDeleted] = @IsDeleted
                OUTPUT INSERTED.*
                WHERE [ForensicReviewProcessId] = @ForensicReviewProcessId
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<ForensicReviewProcess>(sqlCommand, forensicReviewProcess);
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
