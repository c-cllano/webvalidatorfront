using Dapper;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using DrawFlowConfiguration.Domain.Repositories;
using DrawFlowConfiguration.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Infrastructure.Repositories
{
    public class CountriesAndRegionsRepository(SQLServerConnectionFactory connectionFactory) : ICountriesAndRegionsRepository
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();
        public async Task<IEnumerable<CountriesAndRegions?>> GetCountriesAndRegions()
        {
            string sql = "select _pais.CountryId, _pais.Flag , _pais.NameESP , _pais.Indicative , _region.Name AS RegionName , _pais.frecuentcountry" +
                " from country as _pais " +
                " inner join Region as _region  " +
                " on (_region.RegionId = _pais.RegionId)" +
                " where (_pais.Active=1 and _pais.IsDeleted=0) " +
                " and (_region.Active= 1 and _region.IsDeleted =0)";
            return await _connection.QueryAsync<CountriesAndRegions>(sql);
        }
    }
}
