namespace Process.Application.ParametersAgreement.GetById
{
    public record GetParametersAgreementByIdQueryResponse(
        long ParameterAgreementId,
        long AgreementId,
        Guid ParameterAgreementGuid,
        string ParameterName,
        string ParameterValue,
        DateTime CreatedDate,
        DateTime? UpdatedDate,
        bool? Active,
        bool? IsDeleted,
        string ParameterCode,
        string? ParameterDescription
    );
}
