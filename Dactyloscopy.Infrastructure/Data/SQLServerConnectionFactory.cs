using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Dactyloscopy.Infrastructure.Data
{
    public class SQLServerConnectionFactory(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
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
