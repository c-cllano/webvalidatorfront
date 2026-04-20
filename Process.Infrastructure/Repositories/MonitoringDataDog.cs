using Dapper;
using Process.Domain.Parameters.MonitoringDataDog;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class MonitoringDataDog(SQLServerConnectionFactory connectionFactory) : IMonitoringDataDogImg
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;
        public async Task<IEnumerable<MonitoringDataDogResponse>> GetMonitoringDataDogImg(IEnumerable<string> parameterNames)
        {
            string sql = @"SELECT ParameterName,ParameterValue FROM ParameterClient WHERE ParameterName IN @ParameterNames";
            using var connection = CreateConnection();
            return await connection.QueryAsync<MonitoringDataDogResponse>(sql, new { parameterNames });
        }
        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
