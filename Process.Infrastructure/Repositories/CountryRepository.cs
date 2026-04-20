using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class CountryRepository(
        SQLServerConnectionFactory connectionFactory
    ) : ICountryRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<Country?> GetCountryById(int id)
        {
            string sql = @"
                SELECT 
                    [CountryId],[Name],[Indicative],[CreatedDate],
                    [UpdatedDate],[Active],[IsDeleted]
                FROM [dbo].[Country]
                WHERE CountryId = @id
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Country>(sql, new { id });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
