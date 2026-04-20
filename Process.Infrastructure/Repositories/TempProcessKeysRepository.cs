using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class TempProcessKeysRepository(
        SQLServerConnectionFactory connectionFactory
    ) : ITempProcessKeysRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<TempProcessKeys?> GetTempProcessKeysByValidationProcessGuidAsync(Guid validationProcessGuid)
        {
            string sql = @"
                SELECT 
                    [TempProcessKeysId],
                    [ValidationProcessGuid],
                    [PublicKey],
                    [PrivateKey],
                    [AlgorithmPublicKey],
                    [AlgorithmPrivateKey],
                    [CreatedAt],
                    [UpdatedAt]
                FROM [dbo].[TempProcessKeys]
                WHERE ValidationProcessGuid = @validationProcessGuid
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TempProcessKeys>(sql, new { validationProcessGuid });
        }

        public async Task<TempProcessKeys> SaveTempProcessKeysAsync(TempProcessKeys tempProcessKeys)
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[TempProcessKeys](
                    [ValidationProcessGuid]
                   ,[PublicKey]
                   ,[PrivateKey]
                   ,[AlgorithmPublicKey]
                   ,[AlgorithmPrivateKey]
                   ,[CreatedAt]
                   ,[UpdatedAt]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @ValidationProcessGuid
                   ,@PublicKey
                   ,@PrivateKey
                   ,@AlgorithmPublicKey
                   ,@AlgorithmPrivateKey
                   ,@CreatedAt
                   ,@UpdatedAt
                )
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<TempProcessKeys>(sqlCommand, tempProcessKeys);
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
