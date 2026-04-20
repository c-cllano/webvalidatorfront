using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ParametersAgreementRepository(
        SQLServerConnectionFactory connectionFactory
    ) : IParametersAgreementRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<IEnumerable<ParametersAgreement>> GetAllAsync()
        {
            var sql = @"SELECT ParameterAgreementId
                              ,AgreementId
                              ,ParameterAgreementGuid
                              ,ParameterName
                              ,ParameterValue
                              ,CreatedDate
                              ,UpdatedDate
                              ,Active
                              ,IsDeleted
                              ,ParameterCode
                              ,ParameterDescription
                FROM ParametersAgreement WHERE IsDeleted = 0 OR IsDeleted IS NULL";
            using var connection = CreateConnection();

            return await connection.QueryAsync<ParametersAgreement>(sql);
        }

        public async Task<IEnumerable<ParametersAgreement>> GetByAgreementIdAsync(long agreementId)
        {
            var sql = @"
                SELECT 
                    ParameterAgreementId
                    ,AgreementId
                    ,ParameterAgreementGuid
                    ,ParameterName
                    ,ParameterValue
                    ,CreatedDate
                    ,UpdatedDate
                    ,Active
                    ,IsDeleted
                    ,ParameterCode
                    ,ParameterDescription
                FROM ParametersAgreement 
                WHERE AgreementId = @agreementId
            ";

            using var connection = CreateConnection();

            return await connection.QueryAsync<ParametersAgreement>(sql, new { agreementId });
        }

        public async Task<IEnumerable<ParametersAgreement>>
 GetParametersAgreementByAgreementGuidAsync(
     Guid agreementGuid,
     IEnumerable<string>? parameterCodes = null
 )
        {
            var sql = @"
        SELECT 
            pa.ParameterAgreementId,
            pa.AgreementId,
            pa.ParameterAgreementGuid,
            pa.ParameterName,
            pa.ParameterValue,
            pa.CreatedDate,
            pa.UpdatedDate,
            pa.Active,
            pa.IsDeleted,
            pa.ParameterCode,
            pa.ParameterDescription
        FROM ParametersAgreement pa
        INNER JOIN [dbo].[Agreement] a 
            ON a.AgreementId = pa.AgreementId
        WHERE (pa.IsDeleted = 0 OR pa.IsDeleted IS NULL)
        AND a.AgreementGUID = @agreementGuid
    ";

            var parameters = new DynamicParameters();
            parameters.Add("agreementGuid", agreementGuid);

            if (parameterCodes?.Any() == true)
            {
                sql += " AND pa.ParameterCode IN @parameterCodes";
                parameters.Add("parameterCodes", parameterCodes);
            }

            using var connection = CreateConnection();

            return await connection.QueryAsync<ParametersAgreement>(sql, parameters);
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
