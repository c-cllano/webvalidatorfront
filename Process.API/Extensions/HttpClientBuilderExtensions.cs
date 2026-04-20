using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Fallback;
using Polly.Timeout;

namespace Process.API.Extensions
{
    public static class HttpClientBuilderExtensions
    {
        // Inicio código generado por GitHub Copilot
        /// <summary>
        /// Acción de fallback personalizada. Lanza una excepción específica para errores HTTP.
        /// Método generado por GitHub Copilot
        /// </summary>
        public static void AddNamedHttpClientWithDefaultResilience(
        this IServiceCollection services, string clientName, string baseAddress)
        {
            services.AddHttpClient(clientName, client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            })
            .AddResilienceHandler("RetryStrategy", resilienceBuilder =>
            {
                // 1. Estrategia de reintento: SOLO excepciones (HttpRequestException, Timeout, etc.)
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(2),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>() // errores de red
                        .Handle<TimeoutRejectedException>() // timeouts de Polly
                });

                // 2. Fallback: SOLO excepciones (NO status codes)
                resilienceBuilder.AddFallback(new FallbackStrategyOptions<HttpResponseMessage>
                {
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .Handle<TimeoutRejectedException>(),

                    
                    FallbackAction = args =>
                    {
                        throw new HttpRequestException(
                            "Error al realizar la solicitud HTTP. Se agotaron los reintentos o falló el servicio externo.",
                            args.Outcome.Exception
                        );
                    }
                    
                });
            });
        }
        // Fin código generado por GitHub Copilot
    }

}
