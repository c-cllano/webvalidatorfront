namespace Process.Application.VeriFace
{
    public class GetTokenVeriFaceResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
