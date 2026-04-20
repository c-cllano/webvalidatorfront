using Microsoft.Extensions.Logging;
using Nuget.LogService.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Nuget.LogService.Services
{
    public class LogServices(
        string baseUrl,
        string token,
        ILogger<LogServices> logger,
        IHttpClientFactory clientFactory
    ) : ILogServices
    {
        private readonly string _baseUrl = baseUrl;
        private readonly string _token = token;
        private readonly ILogger<LogServices> _logger = logger;
        private readonly IHttpClientFactory _clientFactory = clientFactory;

        public async Task<bool> CreateErrorAsync(CreateErrorIn error)
        {
            return await ExecuteAsync("api/Errors/CreateError", error);
        }

        public async Task<bool> CreateRequestAsync(CreateRequest request)
        {
            return await ExecuteAsync("api/Requests/CreateRequest", request);
        }

        public async Task<bool> ExecuteAsync(string url, object body)
        {
            try
            {
                HttpClient client = _clientFactory.CreateClient("IgnoreSSL");

                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                string json = JsonSerializer.Serialize(body);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.LogError(
                        "Fallo al enviar el log en el servicio centralizado. Código de estado: {StatusCode}.",
                        response.StatusCode
                    );

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Excepción al intentar registrar un log en el servicio centralizado. Detalle: {Message}",
                    ex.Message
                );

                return false;
            }
        }
    }
}
