using ApiGateways.Aplication.LogServices.CreateError;
using ApiGateways.Domain.Services;
using ApiGateways.Infrastructure.Utils;
using MediatR;
using Newtonsoft.Json;
using Polly.CircuitBreaker;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;

namespace ApiGateways.API.Core.Handlers
{
    public class EncodingDelegatingHandler(
        IHttpContextAccessor accessor,
        IReadOnlyDictionary<string, AsyncCircuitBreakerPolicy<HttpResponseMessage>> circuitBreakerPolicies,
        IConfiguration configuration,
        IHttpClientFactory clientFactory,
        ITokenCacheService tokenCacheService
    ) : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor = accessor;
        private readonly IReadOnlyDictionary<string, AsyncCircuitBreakerPolicy<HttpResponseMessage>> _circuitBreakerPolicies = circuitBreakerPolicies;
        private readonly IConfiguration _configuration = configuration;
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly ITokenCacheService _tokenCacheService = tokenCacheService;
        private const string TYPE = "application/json";


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                request.Headers.AcceptEncoding.Clear();
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                // Inicio código generado por GitHub Copilot
                // Manejar endpoint de IP del cliente
                if (IsClientIpRequest())
                {
                    return GetClientIpResponse();
                }
                // Fin código generado por GitHub Copilot

                if (IsThirdPartyApi(request))
                {
                    return await HandleUnifiedTrazabilityAsync(request, cancellationToken);
                }

                string microserviceKey = DetermineMicroservice(request);
                if (!_circuitBreakerPolicies.TryGetValue(microserviceKey, out var circuitBreakerPolicy))
                {
                    return CreateResponse(HttpStatusCode.BadRequest, $"No fue posible determinar la política de Circuit Breaker del microservicio: {microserviceKey}.");
                }
                if (!await HealthyService(microserviceKey))
                {
                    return CreateResponse(HttpStatusCode.BadRequest, "El servicio no está disponible. Intente más tarde.");
                }
                return await circuitBreakerPolicy.ExecuteAsync(async ct => await base.SendAsync(request, ct), cancellationToken);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                return await CreateErrorCommandAsync(HttpStatusCode.RequestTimeout, "La solicitud superó el tiempo límite.", ex);
            }
            catch (BrokenCircuitException ex)
            {
                return await CreateErrorCommandAsync(HttpStatusCode.BadRequest, "El servicio no está disponible. Intente más tarde.", ex);
            }
            catch (OperationCanceledException ex)
            {
                return await CreateErrorCommandAsync(HttpStatusCode.Continue, "La operación fue cancelada.", ex);
            }
            catch (Exception ex)
            {
                return await CreateErrorCommandAsync(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado. Por favor, intente nuevamente.", ex);
            }
        }

        // Inicio código generado por GitHub Copilot
        /// <summary>
        /// Método generado por GitHub Copilot
        /// Determina si la petición es para obtener la IP del cliente
        /// </summary>
        private bool IsClientIpRequest()
        {
            var upstreamPath = _accessor.HttpContext?.Request.Path.Value ?? string.Empty;
            return upstreamPath.Equals("/ClientInfo/GetClientIp", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Método generado por GitHub Copilot
        /// Obtiene la IP real del cliente y retorna una respuesta HTTP
        /// </summary>
        private HttpResponseMessage GetClientIpResponse()
        {
            var context = _accessor.HttpContext;
            if (context == null)
            {
                return CreateResponse(HttpStatusCode.BadRequest, "No se pudo obtener el contexto de la petición");
            }

            string clientIp = GetRealClientIp(context);

            var response = new
            {
                IpAddress = clientIp,
                Timestamp = DateTime.UtcNow
            };

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(response),
                    System.Text.Encoding.UTF8,
                    TYPE)
            };
        }

        /// <summary>
        /// Método generado por GitHub Copilot
        /// Obtiene la IP real del cliente considerando proxies y load balancers
        /// </summary>
        private static string GetRealClientIp(HttpContext context)
        {
            // 1. Intentar obtener desde X-Forwarded-For (usado por proxies y load balancers)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                var ips = forwardedFor.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (ips.Length > 0)
                {
                    return ips[0].Trim(); // La primera IP es la del cliente original
                }

                return context.Connection.RemoteIpAddress?.ToString()!;
            }

            // 2. Intentar obtener desde X-Real-IP (usado por algunos proxies)
            if (context.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
            {
                return realIp.ToString().Trim();
            }

            // 3. Intentar obtener desde CF-Connecting-IP (Cloudflare)
            if (context.Request.Headers.TryGetValue("CF-Connecting-IP", out var cfIp))
            {
                return cfIp.ToString().Trim();
            }

            // 4. Fallback a RemoteIpAddress
            return context.Connection.RemoteIpAddress?.ToString() ?? "IP desconocida";
        }
        // Fin código generado por GitHub Copilot

        private bool IsThirdPartyApi(HttpRequestMessage request)
        {
            string authority = request.RequestUri?.Authority ?? string.Empty;

            var thirdPartyDomain = _configuration.GetValue<string>("RECONOSERID_LOGS_HOST");

            bool isDomainMatch = authority
                .Contains(thirdPartyDomain!, StringComparison.OrdinalIgnoreCase);

            return isDomainMatch;
        }

        private string GetTheServiceURL(string microserviceKey)
        {
            return microserviceKey switch
            {
                Microservice.ProcessAPI => $"{_configuration.GetValue<string>(Microservice.ProcessAPI)}/health",
                Microservice.UIConfigurationAPI => $"{_configuration.GetValue<string>(Microservice.UIConfigurationAPI)}/health",
                Microservice.DactyloscopyAPI => $"{_configuration.GetValue<string>(Microservice.DactyloscopyAPI)}/health",
                Microservice.DrawFlowProcessAPI => $"{_configuration.GetValue<string>(Microservice.DrawFlowProcessAPI)}/health",
                Microservice.DrawFlowConfigurationAPI => $"{_configuration.GetValue<string>(Microservice.DrawFlowConfigurationAPI)}/health",
                Microservice.UITemplateAPI => $"{_configuration.GetValue<string>(Microservice.UITemplateAPI)}/health",
                Microservice.DigitalSignatureAPI => $"{_configuration.GetValue<string>(Microservice.DigitalSignatureAPI)}/health",
                _ => string.Empty
            };
        }

        private string DetermineMicroservice(HttpRequestMessage request)
        {
            string baseUrl = request.RequestUri?.Authority ?? string.Empty;
            return baseUrl switch
            {
                string url when url.Contains(_configuration.GetValue<string>(Microservice.ProcessAPI)?.Replace("http://", "").Replace("https://", "")!) => Microservice.ProcessAPI,
                string url when url.Contains(_configuration.GetValue<string>(Microservice.UIConfigurationAPI)?.Replace("http://", "").Replace("https://", "")!) => Microservice.UIConfigurationAPI,
                string url when url.Contains(_configuration.GetValue<string>(Microservice.DactyloscopyAPI)?.Replace("http://", "").Replace("https://", "")!) => Microservice.DactyloscopyAPI,
                string url when url.Contains(_configuration.GetValue<string>(Microservice.DrawFlowProcessAPI)?.Replace("http://", "").Replace("https://", "")!) => Microservice.DrawFlowProcessAPI,
                string url when url.Contains(_configuration.GetValue<string>(Microservice.DrawFlowConfigurationAPI)?.Replace("http://", "").Replace("https://", "")!) => Microservice.DrawFlowConfigurationAPI,
                string url when url.Contains(_configuration.GetValue<string>(Microservice.UITemplateAPI)?.Replace("http://", "").Replace("https://", "")!) => Microservice.UITemplateAPI,
                string url when url.Contains(_configuration.GetValue<string>(Microservice.DigitalSignatureAPI)?.Replace("http://", "").Replace("https://", "")!) => Microservice.DigitalSignatureAPI,
                _ => string.Empty
            };
        }

        private async Task<bool> HealthyService(string microserviceKey)
        {
            string Url = GetTheServiceURL(microserviceKey);
            if (string.IsNullOrEmpty(Url)) return false;
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync(Url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await CreateErrorCommandAsync(HttpStatusCode.BadRequest, "El servicio no está disponible. Intente más tarde.", ex);
                return false;

            }


        }

        private static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string mensaje)
        {
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mensaje), System.Text.Encoding.UTF8, TYPE)
            };
        }

        private async Task<HttpResponseMessage> CreateErrorCommandAsync(HttpStatusCode statusCode, string mensaje, object exception)
        {
            if (statusCode != HttpStatusCode.Continue)
            {
                IMediator mediator = _accessor.HttpContext?.RequestServices.GetRequiredService<IMediator>()!;
                await mediator.Send(new CreateErrorCommand
                {
                    SeverityID = 5,
                    Description = $"{mensaje}: {exception?.ToString() ?? string.Empty}",
                    Code = statusCode.ToString(),
                    Component = "ApiGateways.API",
                    Date = DateTime.UtcNow.AddHours(-5)
                });
            }
            return CreateResponse(statusCode, mensaje);
        }

        /// <summary>
        /// Método generado por GitHub Copilot
        /// Maneja el flujo unificado: Login + TraceDetailProcess
        /// </summary>
        private async Task<HttpResponseMessage> HandleUnifiedTrazabilityAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                string token = await GetTokenTrazabilityAsync(cancellationToken);

                if (string.IsNullOrEmpty(token))
                {
                    return CreateResponse(
                        HttpStatusCode.Unauthorized,
                        "No se pudo obtener el token de la trazabilidad"
                    );
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return await CreateErrorCommandAsync(
                    HttpStatusCode.InternalServerError,
                    "Error al procesar la petición unificada de Trazabilidad",
                    ex
                );
            }
        }
        // Fin código generado por GitHub Copilot

        /// <summary>
        /// Método refactorizado por GitHub Copilot
        /// Obtiene el token de trazabilidad usando cache
        /// </summary>
        private async Task<string> GetTokenTrazabilityAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                // Intentar obtener el token del cache
                string key = $"Token:TRAZABILITY:{nameof(ApiGateways)}";
                var cachedToken = await _tokenCacheService.GetTokenCacheAsync<string>(key);

                if (cachedToken != null && !string.IsNullOrEmpty(cachedToken))
                {
                    return cachedToken;
                }

                // Si no existe en cache, obtener nuevo token
                var token = await RequestNewTokenTrazabilityAsync(cancellationToken);

                if (!string.IsNullOrEmpty(token))
                {
                    // Guardar en cache (ajusta el tiempo según la respuesta del login)
                    var tokenExpiration = _configuration.GetValue<int?>("TokenCacheExpiredSeconds") ?? 3600;

                    await _tokenCacheService.SetTokenCacheAsync(key, token, tokenExpiration);
                }

                return token;
            }
            catch
            {
                return string.Empty;
            }
        }
        // Fin código generado por GitHub Copilot

        /// <summary>
        /// Método generado por GitHub Copilot
        /// Realiza la petición de login para obtener un nuevo token
        /// </summary>
        private async Task<string> RequestNewTokenTrazabilityAsync(CancellationToken cancellationToken)
        {
            try
            {
                var host = _configuration.GetValue<string>("RECONOSERID_LOGS_HOST");
                var baseUrl = $"https://{host}";

                var loginRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/Api/User/Login")
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(
                            new
                            {
                                Usuario = _configuration.GetValue<string>("Trazabilidad:Usuario"),
                                Contrasena = _configuration.GetValue<string>("Trazabilidad:Contrasena"),
                                Codigo = _configuration.GetValue<string>("Trazabilidad:Codigo")
                            }
                        ),
                        System.Text.Encoding.UTF8,
                        TYPE
                    )
                };

                var loginResponse = await base.SendAsync(loginRequest, cancellationToken);

                if (!loginResponse.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                var loginContent = await loginResponse.Content.ReadAsStringAsync(cancellationToken);
                var tokenObject = JsonConvert.DeserializeObject<dynamic>(loginContent);
                string token = tokenObject?.token ?? tokenObject?.Token ?? tokenObject?.Data.Token!;

                return token ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        // Fin código generado por GitHub Copilot
    }
}
// Fin refactorización/optimización por GitHub Copilot
