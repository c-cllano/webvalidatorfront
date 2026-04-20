
using Process.Domain.Parameters.Auth;

namespace Process.Domain.Services
{
    public interface IAuthService
    {
        Task<string> GetNewToken(string accessToken, Guid procesoConvenioGuid);
        Task<TokenResponseOut> GetToken(string clientId, string clientSecret); 
    }
}
