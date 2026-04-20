using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;

namespace Process.Infrastructure.Repositories
{
    public class AgreementATDPRepository(SQLServerConnectionFactory connectionFactory) : IAgreementATDPRepository
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();
        public async Task<AgreementATDP?> GetMaxAgreement(long agreementId)
        {
            string sql = @" SELECT TOP 1 * FROM ConvenioATDP
                            WHERE ConvenioId = @agreementId
                            ORDER BY Version DESC";

            return await _connection.QueryFirstOrDefaultAsync<AgreementATDP>(sql, new { agreementId });
        }
    }
}
