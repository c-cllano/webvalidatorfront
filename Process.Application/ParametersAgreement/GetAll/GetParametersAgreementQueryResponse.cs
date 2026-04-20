namespace Process.Application.ParametersAgreement.GetAll
{
    public record GetParametersAgreementQueryResponse(
            long ParameterAgreementId,
            long AgreementId,
            Guid ParameterAgreementGuid,
            string ParameterName,
            string ParameterValue,
            DateTime CreatedDate,
            DateTime? UpdatedDate,
            bool? Active,
            bool? IsDeleted
        );
}
