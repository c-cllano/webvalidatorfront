namespace Process.Domain.Entities
{
    public class Token
    {
        public string GrantType { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string? ClientType { get; set; }
    }
}
