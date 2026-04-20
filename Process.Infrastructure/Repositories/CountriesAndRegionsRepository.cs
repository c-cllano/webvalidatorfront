using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{

    public class CountriesAndRegionsRepository(SQLServerConnectionFactory connectionFactory) : ICountriesAndRegionsRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<IEnumerable<CountriesAndRegions?>> GetCountriesAndRegions()
        {

            string sql = "select _pais.CountryId, _pais.Flag , _pais.NameESP , _pais.Indicative , _region.Name AS RegionName , _pais.frecuentcountry" +
                " from country as _pais " +
                " inner join Region as _region  " +
                " on (_region.RegionId = _pais.RegionId)" +
                " where (_pais.Active=1 and _pais.IsDeleted=0) " +
                " and (_region.Active= 1 and _region.IsDeleted =0)";
            using var connection = CreateConnection();
            return await connection.QueryAsync<CountriesAndRegions>(sql);

        }


        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
