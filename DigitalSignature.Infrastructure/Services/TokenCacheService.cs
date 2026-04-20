using DigitalSignature.Domain.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace DigitalSignature.Infrastructure.Services
{
    public class TokenCacheService(
        IDistributedCache cache,
        IConfiguration config
    ) : ITokenCacheService
    {
        private readonly IDistributedCache _cache = cache;
        private readonly IConfiguration _config = config;

        public async Task<T?> GetTokenCacheAsync<T>(string microserviceKey)
        {
            var cachedValue = await _cache.GetAsync(microserviceKey);

            if (cachedValue == null)
                return default!;

            var json = Encoding.UTF8.GetString(cachedValue);
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetTokenCacheAsync<T>(string microserviceKey, T token)
        {
            long expiresInSeconds = long.Parse(_config["TokenCacheExpiredSeconds"]!);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiresInSeconds)
            };

            var json = JsonSerializer.Serialize(token);
            var bytes = Encoding.UTF8.GetBytes(json);

            await _cache.SetAsync(microserviceKey, bytes, options);
        }
    }
}
