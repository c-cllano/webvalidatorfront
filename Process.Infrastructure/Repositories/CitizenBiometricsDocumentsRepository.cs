using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class CitizenBiometricsDocumentsRepository(
        SQLServerConnectionFactory connectionFactory
    ) : ICitizenBiometricsDocumentsRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<CitizenBiometricsDocuments?> GetBiometricsByCitizenIdAndServiceTypeAsync(
            long citizenId,
            int serviceType,
            int? serviceSubType = null
        )
        {
            string sql = @"
                SELECT 
                    [CitizenBiometricsDocumentsId],[CitizenBiometricsDocumentsGuid],[CitizenId],[UrlFile],[ServiceType],
                    [ServiceSubType],[CreatedDate],[UpdatedDate],[Active],[IsDeleted]
                FROM [dbo].[CitizenBiometricsDocuments]
                WHERE CitizenId = @citizenId AND ServiceType = @serviceType AND (ServiceSubType is null OR ServiceSubType = @serviceSubType)
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CitizenBiometricsDocuments>(sql, new { citizenId, serviceType, serviceSubType });
        }

        public async Task<CitizenBiometricsDocuments> SaveBiometricsAsync(
            CitizenBiometricsDocuments biometrics
        )
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[CitizenBiometricsDocuments](
                    [CitizenBiometricsDocumentsGuid],[CitizenId],[UrlFile],[ServiceType],[ServiceSubType],
                    [CreatedDate],[UpdatedDate],[Active],[IsDeleted]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @CitizenBiometricsDocumentsGuid,@CitizenId,@UrlFile,@ServiceType,@ServiceSubType,
                    @CreatedDate,@UpdatedDate,@Active,@IsDeleted
                )
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<CitizenBiometricsDocuments>(sqlCommand, biometrics);
        }

        public async Task<CitizenBiometricsDocuments> UpdateFileAsync(
            long citizenBiometricsDocumentsId,
            string urlFile
        )
        {
            string sqlCommand = @"
                UPDATE [dbo].[CitizenBiometricsDocuments]
                SET 
                    [UrlFile] = @urlFile,
                    [UpdatedDate] = GETDATE()
                OUTPUT INSERTED.*
                WHERE [CitizenBiometricsDocumentsId] = @citizenBiometricsDocumentsId
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<CitizenBiometricsDocuments>(sqlCommand, new { urlFile, citizenBiometricsDocumentsId });
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
