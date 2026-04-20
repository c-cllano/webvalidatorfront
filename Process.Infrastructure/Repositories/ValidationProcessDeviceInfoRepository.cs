using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ValidationProcessDeviceInfoRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IValidationProcessDeviceInfoRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<ValidationProcessDeviceInfo> SaveValidationProcessDeviceInfoAsync(
            ValidationProcessDeviceInfo validationProcessDeviceInfo
        )
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[ValidationProcessDeviceInfo](
                    [ValidationProcessId]
                   ,[MobileDeviceInfoId]
                   ,[BestCameraAutomatic]
                   ,[CreatedDate]
                   ,[UpdatedDate]
                   ,[Active]
                   ,[IsDeleted]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @ValidationProcessId
                   ,@MobileDeviceInfoId
                   ,@BestCameraAutomatic
                   ,@CreatedDate
                   ,@UpdatedDate
                   ,@Active
                   ,@IsDeleted
                )
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<ValidationProcessDeviceInfo>(sqlCommand, validationProcessDeviceInfo);
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
