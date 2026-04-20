using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Repositories
{
    public class ValidationProcessRepository(SQLServerConnectionFactory connectionFactory) : IValidationProcessRepository
    {
        private readonly SQLServerConnectionFactory _factory = connectionFactory;

        public async Task<ValidationProcess?> GetValidationProcessById(long id)
        {
            string sql = @"
                SELECT 
                    [ValidationProcessId],[ValidationProcessGUID],[AgreementGUID],[WorkflowId],[CitizenGUID],[FirstName],[SecondName],[LastName],[SecondLastName],[InfCandidate],
                    [DocumentTypeId],[DocumentNumber],[Email],[CellphoneNumber],[ProcessType],[Approved],[RejectionCauseId],[Advisor],[DocumentIssuingDate],[DocumentIssuingPlace],[OfficeCode],[OfficeName],
                    [ExecuteInMobile],[CreatorUserId],[CreatedDate],[UpdatedDate],[Active],[IsDeleted],[IsCompleted],[CompletionDate],[ForensicState],[Validation],[StatusValidationId],[RequestChannel]
                FROM [dbo].[ValidationProcess]
                WHERE ValidationProcessId = @id
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ValidationProcess>(sql, new { id });
        }

        public async Task<ValidationProcess?> GetValidationProcessByValidationProcessGuid(Guid validationProcessGuid)
        {
            string sql = @"
                SELECT 
                    [ValidationProcessId],[ValidationProcessGUID],[AgreementGUID],[WorkflowId],[CitizenGUID],[FirstName],[SecondName],[LastName],[SecondLastName],[InfCandidate],
                    [DocumentTypeId],[DocumentNumber],[Email],[CellphoneNumber],[ProcessType],[Approved],[RejectionCauseId],[Advisor],[DocumentIssuingDate],[DocumentIssuingPlace],[OfficeCode],[OfficeName],
                    [ExecuteInMobile],[CreatorUserId],[CreatedDate],[UpdatedDate],[Active],[IsDeleted],[IsCompleted],[CompletionDate],[ForensicState],[Validation],[StatusValidationId],[RequestChannel]
                FROM [dbo].[ValidationProcess]
                WHERE Active = 1 AND ValidationProcessGUID = @validationProcessGuid
            ";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ValidationProcess>(sql, new { validationProcessGuid });
        }

        public async Task<ValidationProcess> SaveValidationProcessAsync(ValidationProcess validationProcess)
        {
            string sqlCommand = @"
                INSERT INTO [dbo].[ValidationProcess](
                    [ValidationProcessGUID],[AgreementGUID],[WorkflowId],[CitizenGUID],[FirstName],[SecondName],[LastName],[SecondLastName],[InfCandidate],[DocumentTypeId]
                    ,[DocumentNumber],[Email],[CellphoneNumber],[ProcessType],[Approved],[RejectionCauseId],[Advisor],[DocumentIssuingDate],[DocumentIssuingPlace],[OfficeCode],[OfficeName],[ExecuteInMobile]
                    ,[CreatorUserId],[CreatedDate],[UpdatedDate],[Active],[IsDeleted],[IsCompleted],[CompletionDate],[ForensicState],[Validation],[StatusValidationId],[RequestChannel]
                )
                OUTPUT INSERTED.*
                VALUES(
                    @ValidationProcessGUID,@AgreementGUID,@WorkflowId,@CitizenGUID,@FirstName,@SecondName,@LastName,@SecondLastName,@InfCandidate,@DocumentTypeId,@DocumentNumber,@Email
                    ,@CellphoneNumber,@ProcessType,@Approved,@RejectionCauseId,@Advisor,@DocumentIssuingDate,@DocumentIssuingPlace,@OfficeCode,@OfficeName,@ExecuteInMobile,@CreatorUserId,@CreatedDate
                    ,@UpdatedDate,@Active,@IsDeleted,@IsCompleted,@CompletionDate,@ForensicState,@Validation,@StatusValidationId,@RequestChannel
                )
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<ValidationProcess>(sqlCommand, validationProcess);
        }

        public async Task<ValidationProcess> UpdateValidationProcessAsync(ValidationProcess validationProcess)
        {
            string sqlCommand = @"
                UPDATE [dbo].[ValidationProcess]
                SET
                    [ValidationProcessGUID] = @ValidationProcessGUID,
                    [AgreementGUID] = @AgreementGUID,
                    [WorkflowId] = @WorkflowId,
                    [CitizenGUID] = @CitizenGUID,
                    [FirstName] = @FirstName,
                    [SecondName] = @SecondName,
                    [LastName] = @LastName,
                    [SecondLastName] = @SecondLastName,
                    [InfCandidate] = @InfCandidate,
                    [DocumentTypeId] = @DocumentTypeId,
                    [DocumentNumber] = @DocumentNumber,
                    [Email] = @Email,
                    [CellphoneNumber] = @CellphoneNumber,
                    [ProcessType] = @ProcessType,
                    [Approved] = @Approved,
                    [RejectionCauseId] = @RejectionCauseId,
                    [Advisor] = @Advisor,
                    [DocumentIssuingDate] = @DocumentIssuingDate,
                    [DocumentIssuingPlace] = @DocumentIssuingPlace,
                    [OfficeCode] = @OfficeCode,
                    [OfficeName] = @OfficeName,
                    [ExecuteInMobile] = @ExecuteInMobile,
                    [CreatorUserId] = @CreatorUserId,
                    [CreatedDate] = @CreatedDate,
                    [UpdatedDate] = @UpdatedDate,
                    [Active] = @Active,
                    [IsDeleted] = @IsDeleted,
                    [IsCompleted] = @IsCompleted,
                    [CompletionDate] = @CompletionDate,
                    [ForensicState] = @ForensicState,
                    [ForensicReason] = @ForensicReason,
                    [ForensicOptionalReason] = @ForensicOptionalReason,
                    [ForensicObservations] = @ForensicObservations,
                    [Validation] = @Validation,
                    [StatusValidationId] = @StatusValidationId,
                    [RequestChannel] = @RequestChannel
                OUTPUT INSERTED.*
                WHERE [ValidationProcessId] = @validationProcessId
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<ValidationProcess>(sqlCommand, validationProcess);
        }

        private IDbConnection CreateConnection()
        {
            return _factory.CreateConnection(ConnectionsName.OKeyConnection);
        }
    }
}
