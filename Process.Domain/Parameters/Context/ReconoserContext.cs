namespace Process.Domain.Parameters.Context
{
    public sealed class ReconoserContext
    {
        public bool? ChangeUrl { get; set; }
        public string? BaseUrlReconoser1 { get; set; }
        public string? BaseUrlReconoser2 { get; set; }
        public Guid AgreementGUID { get; set; }
    }
}
