using Microsoft.Data.SqlClient;
using System.Data;

namespace DrawFlowConfiguration.Infrastructure.Data
{
    public class SQLServerConnectionFactory
    {
        private readonly string _connectionString;

        public SQLServerConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
