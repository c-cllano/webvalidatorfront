using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;

namespace Process.Infrastructure.Repositories
{
    public class BiometricKeysTempRepository(SQLServerConnectionFactory connectionFactory) : IBiometricKeysTempRepository
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();

        public async Task<BiometricKey?> GetByProcessAgreementGuidAsync(Guid processAgreementGuid)
        {
            const string sql = @"
                SELECT lbt.*
                FROM transaccional.LlavesBiometricasTemp lbt
                INNER JOIN ProcesoConvenio pc ON lbt.ProcesoConvenioGuid = pc.ProcesoConvenioGuid
                WHERE lbt.ProcesoConvenioGuid = @ProcessAgreementGuid";


            return await _connection.QueryFirstOrDefaultAsync<BiometricKey?>(
                sql, new { ProcessAgreementGuid = processAgreementGuid });
        }

        public async Task<long> AddNewItemAsync(BiometricKey data)
        {
            const string sql = @"
                INSERT INTO transaccional.LlavesBiometricasTemp
                (ProcesoConvenioGuid, LlavePublica, LlavePrivada, AlgorithmPrivateKey, AlgorithmPublicKey, CreatedAt, UpdatedAt)
                VALUES (@ProcesoConvenioGuid, @LlavePublica, @LlavePrivada, @AlgorithmPrivateKey, @AlgorithmPublicKey, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() as bigint);";


            var id = await _connection.ExecuteScalarAsync<long>(sql, data);
            return id;
        }
    }
}
