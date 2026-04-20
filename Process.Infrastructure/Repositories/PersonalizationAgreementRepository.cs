using Dapper;
using Process.Domain.Parameters.PersonalizationAgreement;
using Process.Infrastructure.Data;
using System.Data;

namespace Process.Infrastructure.Repositories
{
    public class PersonalizationAgreementRepository(SQLServerConnectionFactory connectionFactory) : Domain.Repositories.IPersonalizationAgreementRepository
    {

        private readonly IDbConnection _connection = connectionFactory.CreateConnection();

        public async Task<List<GetFrontConfigurationOut>> GetFrontConfiguration(long procesoConvenioId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ConvenioId", procesoConvenioId);
            const string sql = "SELECT p.Nombre, cp.Valor, p.PersonalizacionId as PersonalizacionId FROM convenio.ConvenioPersonalizacion cp " +
                "inner join convenio.Personalizacion p on cp.PersonalizacionId  = p.PersonalizacionId" +
                " where cp.ConvenioId = @ConvenioId";

            var personalizations = await _connection.QueryAsync<GetFrontConfigurationOut>(sql, parameters);
            return [.. personalizations];
        }

    }
}
