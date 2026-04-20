namespace Dactyloscopy.Domain.Parameters.ExternalApiClientParameters
{
    public class ForensicObjectRequest
    {
        public string ObjectType { get; set; } = string.Empty;
        public string Object { get; set; } = string.Empty;
        public string ObjectFormat { get; set; } = string.Empty;
    }
}
