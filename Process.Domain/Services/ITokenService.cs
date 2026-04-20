using System.Security.Claims;

namespace Process.Domain.Services
{
    public interface ITokenService
    {
        IEnumerable<Claim> GetClaims();
        string GetClaim(string claimType);
    }
}
