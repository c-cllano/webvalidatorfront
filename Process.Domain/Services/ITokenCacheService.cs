namespace Process.Domain.Services
{
    public interface ITokenCacheService
    {
        Task<T?> GetTokenCacheAsync<T>(string microserviceKey);
        Task SetTokenCacheAsync<T>(string microserviceKey, T token, long? expiresInSeconds = null);
    }
}
