using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;

namespace Process.Infrastructure.Repositories
{
    public class AgreementRepository(SQLServerConnectionFactory connectionFactory) : IAgreementRepository
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();
        public async Task<Agreement?> AgreementById(long agreementId)
        {
            string sql = "SELECT * FROM convenio.Convenio WHERE ConvenioId = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Agreement>(sql, new { Id = agreementId });
        }
    }
}
