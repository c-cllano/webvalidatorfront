using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UIConfiguration.Domain.Parameters.Auth;
using UIConfiguration.Infrastructure.Utility;
using System.IdentityModel.Tokens.Jwt;

namespace UIConfiguration.Infrastructure.Services
{
    public class AuthService : Domain.Services.IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly string _url;
        private readonly string _aeskeyBiometric;
        private readonly string _thumbprintCertifacte;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
            _url = _config.GetSection("IdentityServer:Token")?.Value ?? string.Empty;
            _aeskeyBiometric = _config.GetSection("AESKeyBiometric")?.Value ?? string.Empty;
            _thumbprintCertifacte = _config.GetSection("thumbprintCertifacte")?.Value ?? string.Empty;
        }

        public async Task<string> GetNewToken(string accessToken, Guid procesoConvenioGuid)
        {
            var procesoConvenioEncry = procesoConvenioGuid.ToString().EncryptToAESNew(_aeskeyBiometric);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenAux = tokenHandler.ReadJwtToken(accessToken);
            var claimList = tokenAux.Claims.ToList();
            claimList.Add(new System.Security.Claims.Claim("process", $"{procesoConvenioEncry.Item1}|{procesoConvenioEncry.Item2}"));
            X509SigningCredentials clientSigningCredentials = new(GetCertificadoSigningCredentials.GetCertificateInfo(this._thumbprintCertifacte));
            var newToken = new JwtSecurityToken(tokenAux.Issuer, tokenAux.Audiences.FirstOrDefault(), claimList, tokenAux.ValidFrom, tokenAux.ValidTo, clientSigningCredentials);
            return await Task.Run(() => tokenHandler.WriteToken(newToken));
        }

        public async Task<TokenResponseOut> GetToken(string clientId, string clientSecret)
        {
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new FormUrlEncodedContent(
                [
                new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
            ])
            };

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener el token: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponseOut>(content);

            return tokenResponse!;
        }
    }
}
