
namespace UIConfiguration.Application.Auth.GetToken
{
    public class GetTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string ErrorDescription { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
