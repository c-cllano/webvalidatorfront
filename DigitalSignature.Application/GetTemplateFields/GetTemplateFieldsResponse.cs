namespace DigitalSignature.Application.GetTemplateFields
{
    public class GetTemplateFieldsResponse
    {
        public bool Ok { get; set; }
        public IEnumerable<string> Annotations { get; set; } = default!;
    }
}
