using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ClientRepository(SQLServerConnectionFactory factory) : IClientRepository
    {
        private readonly SQLServerConnectionFactory _factory = factory;
        public async Task<Client?> GetClientByToken(Guid clientToken)
        {
            var sql = @"
        SELECT [ClientId]
              ,[ClientToken]
              ,[Name]
              ,[DocumentTypeId]
              ,[DocumentNumber]
              ,[EconomicSectorId]
              ,[CountryId]
              ,[LegalRepresentativeId]
              ,[CreatorUserId]
              ,[CreatedDate]
              ,[UpdatedDate]
              ,[Active]
              ,[IsDeleted]
        FROM [dbo].[Client]
        WHERE ClientToken = @ClientToken;
    ";

            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Client>(sql, new { ClientToken = clientToken });

        }

        public Task<Client> GetClient(long clientId)
        {
            var sql = @"SELECT [ClientId]
                          ,[ClientToken]
                          ,[Name]
                          ,[DocumentTypeId]
                          ,[DocumentNumber]
                          ,[EconomicSectorId]
                          ,[CountryId]
                          ,[LegalRepresentativeId]
                          ,[CreatorUserId]
                          ,[CreatedDate]
                          ,[UpdatedDate]
                          ,[Active]
                          ,[IsDeleted]
                      FROM [dbo].[Client]
                      WHERE  ClientId = @ClientId    
                ";
            using var connection = CreateConnection();
            var result = connection.QueryFirstAsync<Client>(sql, new { clientId });
            return result;
        }


        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }

    }
}
