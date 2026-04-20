namespace Process.Domain.Entities
{
    public class CreateClient
    {
        public string ClientId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ClientType { get; set; } = "Public";
        public string? ClientSecret { get; set; }
    }
}
