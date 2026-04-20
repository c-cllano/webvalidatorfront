namespace DigitalSignature.Application.GenerateDocument
{
    public class GenerateDocumentResponse
    {
        public bool Ok { get; set; }
        public string DocumentBase64 { get; set; } = string.Empty;
    }
}
