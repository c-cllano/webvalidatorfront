using DigitalSignature.Domain.Exceptions;
using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using DigitalSignature.Domain.Utilities;
using Microsoft.Extensions.Configuration;
using Nuget.LogService.Models;
using Nuget.LogService.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DigitalSignature.Infrastructure.Services
{
    public class ExternalApiClientService(
        IHttpClientFactory clientFactory,
        IConfiguration config,
        ILogServices logService
    ) : IExternalApiClientService
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IConfiguration _config = config;
        private readonly ILogServices _logService = logService;

        public async Task<T> GetAsync<T>(ExternalApiClientRequest externalApiClientRequest)
        {
            return await SendAsync<T>(externalApiClientRequest, HttpMethod.Get);
        }

        public async Task<T> PostAsync<T>(ExternalApiClientRequest externalApiClientRequest)
        {
            return await SendAsync<T>(externalApiClientRequest, HttpMethod.Post);
        }

        public async Task<T> PutAsync<T>(ExternalApiClientRequest externalApiClientRequest)
        {
            return await SendAsync<T>(externalApiClientRequest, HttpMethod.Put);
        }

        // Inicio código generado por GitHub Copilot
        // Método generado por GitHub Copilot
        private async Task<T> SendAsync<T>(
            ExternalApiClientRequest externalApiClientRequest,
            HttpMethod method
        )
        {
            HttpClient client = _clientFactory.CreateClient("IgnoreSSL");
            string url = GetApiUrl(externalApiClientRequest, method);

            HttpContent? content = BuildContent(externalApiClientRequest);

            HttpRequestMessage request = new(method, url)
            {
                Content = content
            };

            AddAuthorizationAndCustomHeaders(request, externalApiClientRequest);

            HttpResponseMessage response = await client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();

            ThreadExecutionHelper.ThreadExecution(() => CreateRequestLogAsync(externalApiClientRequest, result, url, response, request));

            return await HandleResponseAsync<T>(response, result);
        }

        // Método generado por GitHub Copilot
        private static HttpContent? BuildContent(ExternalApiClientRequest externalApiClientRequest)
        {
            if (externalApiClientRequest.Files != null && externalApiClientRequest.Files.Count != 0)
            {
                var multipartContent = new MultipartFormDataContent();

                if (externalApiClientRequest.Body != null)
                {
                    string json = JsonSerializer.Serialize(externalApiClientRequest.Body);
                    multipartContent.Add(new StringContent(json, Encoding.UTF8, "application/json"), "payload");
                }

                foreach (var file in externalApiClientRequest.Files)
                {
                    var extension = ImageHelper.GetImageExtension(file.Value);
                    var contentType = ImageHelper.GetMimeType(extension);
                    var fileName = $"{file.Key}{extension}";

                    var fileContent = new ByteArrayContent(file.Value);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                    multipartContent.Add(fileContent, file.Key, fileName);
                }

                return multipartContent;
            }

            if (externalApiClientRequest.Body != null)
            {
                return new StringContent(
                    JsonSerializer.Serialize(externalApiClientRequest.Body),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            return null;
        }

        // Método generado por GitHub Copilot
        private static void AddAuthorizationAndCustomHeaders(HttpRequestMessage request, ExternalApiClientRequest externalApiClientRequest)
        {
            if (!string.IsNullOrEmpty(externalApiClientRequest.Token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    string.IsNullOrEmpty(externalApiClientRequest.AuthorizationValue) ? "Bearer" : externalApiClientRequest.AuthorizationValue,
                    externalApiClientRequest.Token
                );
            }

            if (externalApiClientRequest.CustomHeaders != null)
            {
                foreach (var header in externalApiClientRequest.CustomHeaders)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        // Método generado por GitHub Copilot
        private static async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, string result)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (TryDeserialize(result, out T? resultResponseWithError) && resultResponseWithError != null)
                {
                    return await Task.FromResult(resultResponseWithError);
                }

                if (TryDeserialize(result, out ExternalApiError? resultError) && resultError != null)
                {
                    throw new ExternalApiException(resultError);
                }

                throw new IntegrationException($"Error al procesar la solicitud: Status: {(int)response.StatusCode}, Description: {response.ReasonPhrase}");
            }

            return await Task.FromResult(JsonSerializer.Deserialize<T>(result)!);
        }
        // Fin código generado por GitHub Copilot

        private async Task CreateRequestLogAsync(
            ExternalApiClientRequest externalApiClientRequest,
            string result,
            string url,
            HttpResponseMessage response,
            HttpRequestMessage request
        )
        {
            string requestString = externalApiClientRequest.Body != null
                ? JsonSerializer.Serialize(externalApiClientRequest.Body)
                : string.Empty;

            if (externalApiClientRequest.BodyExternalRequest != null)
            {
                requestString = JsonSerializer.Serialize(externalApiClientRequest.BodyExternalRequest);
            }

            CreateRequest logRequest = new()
            {
                Request = requestString,
                Response = result,
                Type = request.Method.Method,
                Endpoint = url,
                Status = (int)response.StatusCode,
                StartingTime = DateTime.UtcNow.AddHours(-5),
                FinalTime = DateTime.UtcNow.AddHours(-5),
                Component = "ExternalApiClientService",
                TransactionID = Guid.NewGuid().ToString(),
                Machine = Environment.MachineName,
                UserID = Environment.UserName,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _logService.CreateRequestAsync(logRequest);
        }

        private string GetApiUrl(
            ExternalApiClientRequest externalApiClientRequest,
            HttpMethod method
        )
        {
            string baseUrl = _config.GetSection($"{externalApiClientRequest.Section}:{externalApiClientRequest.BaseUrl}")?.Value!;
            string apiName = _config.GetSection($"{externalApiClientRequest.Section}:{externalApiClientRequest.ApiName}")?.Value!;
            string port = _config.GetSection($"{externalApiClientRequest.Section}:{externalApiClientRequest.Port}")?.Value!;

            string urlReturn = string.IsNullOrWhiteSpace(port)
                ? $"{baseUrl}/{apiName}"
                : $"{baseUrl}:{port}/{apiName}";

            if (externalApiClientRequest.QueryParams != null && method == HttpMethod.Get)
            {
                var queryParams = string.Join(
                    "&",
                    externalApiClientRequest.QueryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}")
                );

                urlReturn = $"{urlReturn}?{queryParams}";
            }

            return urlReturn;
        }

        private static bool TryDeserialize<T>(string json, out T? result)
        {
            result = default;

            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            try
            {
                result = JsonSerializer.Deserialize<T>(json);

                return result != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
