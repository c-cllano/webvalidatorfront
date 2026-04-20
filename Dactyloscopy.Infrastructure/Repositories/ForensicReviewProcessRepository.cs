using Dactyloscopy.Domain.Entities;
using Dactyloscopy.Domain.Repositories;
using Dactyloscopy.Infrastructure.Data;
using Dapper;
using System.Data;

namespace Dactyloscopy.Infrastructure.Repositories
{
    public class ForensicReviewProcessRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IForensicReviewProcessRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<ForensicReviewProcess?> GetForensicReviewProcessByIdAsync(long forensicReviewProcessId)
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
                  WHERE ForensicReviewProcessId = @forensicReviewProcessId
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ForensicReviewProcess>(sql, new { forensicReviewProcessId });
        }

        public async Task<IEnumerable<ForensicReviewProcess>> GetForensicReviewProcessInReviewAsync(long statusForensicId)
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
                  WHERE StatusForensicId = @statusForensicId
            ";

            using var connection = CreateConnection();
            return await connection.QueryAsync<ForensicReviewProcess>(sql, new { statusForensicId });
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
            return _factory.CreateConnection();
        }
    }
}
