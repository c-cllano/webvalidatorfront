using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static DigitalSignature.Domain.Enums.Enumerations;

namespace DigitalSignature.Infrastructure.Data
{
    public class SQLServerConnectionFactory(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public IDbConnection CreateConnection(ConnectionsName? connectionNameEnum = null)
        {
            connectionNameEnum ??= ConnectionsName.DefaultConnection;

            var connectionString = _configuration.GetConnectionString(connectionNameEnum.ToString()!);
            connectionString = NormalizeBackslashes(connectionString!);

            return new SqlConnection(connectionString);
        }

        private static string NormalizeBackslashes(string value)
        {
            while (value.Contains(@"\\"))
            {
                value = value.Replace(@"\\", @"\");
            }

            return value;
        }
    }
}
