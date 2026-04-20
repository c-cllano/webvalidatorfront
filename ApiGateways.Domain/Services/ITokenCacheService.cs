// Inicio código generado por GitHub Copilot
namespace ApiGateways.Domain.Services
{
    /// <summary>
    /// Interfaz generada por GitHub Copilot
    /// Define el contrato para el servicio de cache de tokens
    /// </summary>
    public interface ITokenCacheService
    {
        Task<T?> GetTokenCacheAsync<T>(string cacheKey);
        Task SetTokenCacheAsync<T>(string cacheKey, T token, long? expiresInSeconds = null);
    }
}
// Fin código generado por GitHub Copilot
