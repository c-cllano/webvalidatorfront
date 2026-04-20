// Inicio código generado por GitHub Copilot
using ApiGateways.Domain.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace ApiGateways.Infrastructure.Services
{
    public class TokenCacheService(
        IDistributedCache cache,
        IConfiguration config
    ) : ITokenCacheService
    {
        private readonly IDistributedCache _cache = cache;
        private readonly IConfiguration _config = config;
        private const long DefaultExpirationSeconds = 3600;

        // Método generado por GitHub Copilot
        public async Task<T?> GetTokenCacheAsync<T>(string cacheKey)
        {
            var cachedValue = await _cache.GetAsync(cacheKey);

            if (cachedValue == null)
                return default;

            var json = Encoding.UTF8.GetString(cachedValue);
            return JsonSerializer.Deserialize<T>(json);
        }

        // Método generado por GitHub Copilot
        public async Task SetTokenCacheAsync<T>(string cacheKey, T token, long? expiresInSeconds = null)
        {
            expiresInSeconds ??= _config.GetValue<long?>("TokenCacheExpiredSeconds") ?? DefaultExpirationSeconds;

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiresInSeconds.Value)
            };

            var json = JsonSerializer.Serialize(token);
            var bytes = Encoding.UTF8.GetBytes(json);

            await _cache.SetAsync(cacheKey, bytes, options);
        }
    }
}
// Fin código generado por GitHub Copilot
