namespace Process.Application.Agreements.GetByClient
{
    public record GetAgreementsByClientGuidQueryResponse(
     long AgreementId,
     Guid AgreementGuid,
     string Name
    );

}
