using Dapper;
using DigitalSignature.Domain.Entities;
using DigitalSignature.Domain.Repositories;
using DigitalSignature.Infrastructure.Data;
using System.Data;
using static DigitalSignature.Domain.Enums.Enumerations;

namespace DigitalSignature.Infrastructure.Repositories
{
    public class ParameterClientRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IParameterClientRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<IEnumerable<ParameterClient>> GetParameterClientByClientId(long clientId)
        {
            string sql = @"
                SELECT 
                    [ParameterClientId],[ParameterClientGuid],[ClientId],[ParameterName],[ParameterValue],
                    [CreatedDate],[UpdatedDate],[Active],[IsDeleted]
                FROM [dbo].[ParameterClient]
                WHERE ClientId = @clientId
            ";

            using var connection = CreateConnection();
            return await connection.QueryAsync<ParameterClient>(sql, new { clientId });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
