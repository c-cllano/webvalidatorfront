using Dapper;
using Process.Domain.Entities;
using Process.Infrastructure.Data;
using System.Data;

namespace Process.Infrastructure.Repositories
{
    public class AgreementProcessRepository(SQLServerConnectionFactory connectionFactory) : Domain.Repositories.IAgreementProcessRepository
    {

        private readonly IDbConnection _connection = connectionFactory.CreateConnection();

        public async Task<AgreementProcess> GetProcess(Guid procesoConvenioGuid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ProcesoConvenioGuid", procesoConvenioGuid);
            const string sql = "SELECT * FROM [convenio].[ProcesoConvenio] WITH(NOLOCK) WHERE ProcesoConvenioGuid = @ProcesoConvenioGuid";
            return await _connection.QuerySingleOrDefaultAsync<AgreementProcess>(
                sql, parameters);           
        }

    }
}
