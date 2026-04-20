using Microsoft.AspNetCore.Http;
using Process.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Process.Infrastructure.Services
{
    public class TokenService(
        IHttpContextAccessor httpContextAccessor
    ) : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public IEnumerable<Claim> GetClaims()
        {
            return LoadClaims();
        }

        public string GetClaim(string claimType)
        {
            return LoadClaims().FirstOrDefault(c => c.Type == claimType)?.Value ?? "Default";
        }

        private IEnumerable<Claim> LoadClaims()
        {
            var context = _httpContextAccessor.HttpContext;

            var authHeader = context?.Request.Headers["Authorization"].FirstOrDefault();

            var token = authHeader!.StartsWith("Bearer ")
                ? authHeader["Bearer ".Length..].Trim()
                : authHeader;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return [.. jwt.Claims];
        }
    }
}
