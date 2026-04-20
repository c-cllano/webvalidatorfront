namespace DigitalSignature.Application.GetTemplate
{
    public class GetTemplateResponse
    {
        public bool Ok { get; set; }
        public string DocumentBase64 { get; set; } = string.Empty;
    }
}
