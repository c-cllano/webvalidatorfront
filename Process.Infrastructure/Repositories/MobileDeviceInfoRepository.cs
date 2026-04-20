using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class MobileDeviceInfoRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IMobileDeviceInfoRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<MobileDeviceInfo> CreateUpdateMobileDeviceInfoAsync(
            MobileDeviceInfo mobileDeviceInfo
        )
        {
            var mobileDeviceInfoCreateUpdate = await GetMobileDeviceInfoByModelAsync(
                mobileDeviceInfo.Model,
                mobileDeviceInfo.OS,
                mobileDeviceInfo.OSVersion,
                mobileDeviceInfo.Browser
            );

            if (mobileDeviceInfoCreateUpdate == null)
            {
                mobileDeviceInfoCreateUpdate = await CreateMobileDeviceInfoAsync(mobileDeviceInfo!);
            }
            else
            {
                mobileDeviceInfoCreateUpdate.MediaDeviceInfo = mobileDeviceInfo.MediaDeviceInfo;
                mobileDeviceInfoCreateUpdate.UpdatedDate = DateTime.UtcNow.AddHours(-5);

                mobileDeviceInfoCreateUpdate = await UpdateMobileDeviceInfoAsync(mobileDeviceInfoCreateUpdate);
            }

            return mobileDeviceInfoCreateUpdate;
        }

        private async Task<MobileDeviceInfo> CreateMobileDeviceInfoAsync(MobileDeviceInfo mobileDeviceInfo)
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[MobileDeviceInfo](
                    [Brand]
                   ,[Model]
                   ,[MediaDeviceInfo]
                   ,[BestCameraManual]
                   ,[CreatedDate]
                   ,[UpdatedDate]
                   ,[Active]
                   ,[IsDeleted]
                   ,[OS]
                   ,[OSVersion]
                   ,[Browser]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @Brand
                   ,@Model
                   ,@MediaDeviceInfo
                   ,@BestCameraManual
                   ,@CreatedDate
                   ,@UpdatedDate
                   ,@Active
                   ,@IsDeleted
                   ,@OS
                   ,@OSVersion
                   ,@Browser
                )
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<MobileDeviceInfo>(sqlCommand, mobileDeviceInfo);
        }

        private async Task<MobileDeviceInfo> UpdateMobileDeviceInfoAsync(MobileDeviceInfo mobileDeviceInfo)
        {
            string sqlCommand = @"
                UPDATE [dbo].[MobileDeviceInfo]
                SET 
                    [Brand] = @Brand
                    ,[Model] = @Model
                    ,[MediaDeviceInfo] = @MediaDeviceInfo
                    ,[BestCameraManual] = @BestCameraManual
                    ,[UpdatedDate] = @UpdatedDate
                    ,[Active] = @Active
                    ,[IsDeleted] = @IsDeleted
                    ,[OS] = @OS
                    ,[OSVersion] = @OSVersion
                    ,[Browser] = @Browser
                OUTPUT INSERTED.*
                WHERE [MobileDeviceInfoId] = @MobileDeviceInfoId
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<MobileDeviceInfo>(sqlCommand, mobileDeviceInfo);
        }

        public async Task<MobileDeviceInfo?> GetMobileDeviceInfoByModelAsync(
            string model,
            string os,
            string osVersion,
            string browser
        )
        {
            string sql = @"
                SELECT 
                    [MobileDeviceInfoId]
                    ,[Brand]
                    ,[Model]
                    ,[MediaDeviceInfo]
                    ,[BestCameraManual]
                    ,[CreatedDate]
                    ,[UpdatedDate]
                    ,[Active]
                    ,[IsDeleted]
                    ,[OS]
                    ,[OSVersion]
                    ,[Browser]
                FROM [dbo].[MobileDeviceInfo]
                WHERE Model = @model
                AND OS = @os
                AND OSVersion = @osVersion
                AND Browser = @browser
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<MobileDeviceInfo>(sql, new { model, os, osVersion, browser });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
