namespace Process.Application.AgreementByUser.GetByGuid
{
    public class GetByGuidQueryResponse
    {
        public Guid AgreementGuid { get; set; }
        public int AgreementId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserReconoserId { get; set; } = string.Empty;
    }
}
