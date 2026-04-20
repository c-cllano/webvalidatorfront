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

    public class AgreementByGuidRepository(SQLServerConnectionFactory connectionFactory) : IAgreementByGuidRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<IEnumerable<AgreementByGuid>> GetByGuidAsync(Guid guid)
        {
            var sql = @" SELECT AgreementGUID, AgreementId, ClientId, Name, UserReconoserId
                         FROM Agreement
                         WHERE AgreementGUID = @Guid
                         AND ( Active = 1 AND IsDeleted = 0 )";
            using var connection = CreateConnection();
            return await connection.QueryAsync<AgreementByGuid>(sql, new { guid = guid });
        }


        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }

    }
}

