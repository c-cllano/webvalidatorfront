using UIConfiguration.Domain.Parameters.Auth;
namespace UIConfiguration.Domain.Services
{
    public interface IAuthService
    {
        Task<string> GetNewToken(string accessToken, Guid procesoConvenioGuid);
        Task<TokenResponseOut> GetToken(string clientId, string clientSecret);
    }
}
