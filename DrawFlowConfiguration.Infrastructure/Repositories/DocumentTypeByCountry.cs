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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DrawFlowConfiguration.Infrastructure.Repositories
{
    public class DocumentTypeByCountryRepository(SQLServerConnectionFactory connectionFactory) : IDocumentTypeByCountryRepository
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();
        public async Task<IEnumerable<DocumentTypeByCountry>> GetDocumentTypeByCountry()
        {

            string sql = "select  _dtc.DocumentTypeByCountryId, " +
                "_c.CountryId,  _c.Flag, _c.Name as NameCountry , _C.NameESP , _c.Indicative, _c.frecuentCountry, " +
                "_dt.DocumentTypeId, _dt.Code  as CodeDocumentType, _dt.Name as NameDocumentType, " +
                "_rg.RegionId, _rg.Name as NameRegion , _dt.Active, _dt.Length, _dt.RegularExpression , _dt.MinLength, _dt.MaxLength " +
                "from DocumentTypeByCountry _dtc " +
                "inner join Country _c on (_c.CountryId = _dtc.CountryId) " +
                "inner join DocumentType _dt on (_dt.DocumentTypeId = _dtc.DocumentTypeId) " +
                "inner join Region _rg on (_rg.RegionId = _c.RegionId) " +
                "where (_dtc.Active=1 and _dtc.IsDeleted=0 ) " +
                "and (_c.Active=1 and _c.IsDeleted=0) " +
                "and (_dt.Active=1 and _dt.IsDeleted=0) " +
                "and(_rg.Active=1 and _rg.IsDeleted=0)";
            return await _connection.QueryAsync<DocumentTypeByCountry>(sql);
        }
    }
}
