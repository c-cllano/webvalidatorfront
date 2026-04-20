using Microsoft.Extensions.Configuration;
using Nuget.LogService.Models;
using Nuget.LogService.Services;
using Process.Domain.Exceptions;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using Process.Domain.Utilities;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Process.Infrastructure.Services
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

        private static readonly SemaphoreSlim _inflight = new(50);

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

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

        public async Task DeleteAsync<T>(ExternalApiClientRequest externalApiClientRequest)
        {
            await SendAsync<T>(externalApiClientRequest, HttpMethod.Delete);
        }

        // Inicio refactorización/optimización por GitHub Copilot
        private async Task<T> SendAsync<T>(
            ExternalApiClientRequest externalApiClientRequest,
            HttpMethod method
        )
        {
            await _inflight.WaitAsync();

            try
            {
                HttpClient client = _clientFactory.CreateClient("IgnoreSSL");

                string url = externalApiClientRequest.Url ?? GetApiUrl(externalApiClientRequest, method);
                string bodyJson = GetRequestBodyJson(externalApiClientRequest);

                HttpContent? content = GetContentAsync(externalApiClientRequest, bodyJson);

                using var request = PrepareRequest(method, url, content);
                AddAuthorizationAndCustomHeaders(request, externalApiClientRequest);

                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                string result;
                await using (var responseStream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    result = await reader.ReadToEndAsync();
                }

                _ = Task.Run(() => CreateRequestLogAsync(
                    externalApiClientRequest,
                    result,
                    url,
                    response,
                    request,
                    bodyJson
                ));

                bodyJson = null!;

                return await ProcessResponseAsync<T>(response, result);
            }
            finally
            {
                _inflight.Release();
            }
        }

        private static string TruncateLargePayload(string input, int maxLength = 10_000)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Length > maxLength
                ? input[..maxLength] + " ...[TRUNCATED]"
                : input;
        }

        private static string GetRequestBodyJson(ExternalApiClientRequest externalApiClientRequest)
        {
            return externalApiClientRequest.Body != null
                ? JsonSerializer.Serialize(externalApiClientRequest.Body, _jsonOptions)
                : string.Empty;
        }

        // Método generado por GitHub Copilot
        private static HttpRequestMessage PrepareRequest(HttpMethod method, string url, HttpContent? content)
        {
            return new HttpRequestMessage(method, url)
            {
                Content = content
            };
        }

        // Método generado por GitHub Copilot
        private static void AddAuthorizationAndCustomHeaders(HttpRequestMessage request, ExternalApiClientRequest externalApiClientRequest)
        {
            if (!string.IsNullOrEmpty(externalApiClientRequest.Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
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
        private static async Task<T> ProcessResponseAsync<T>(HttpResponseMessage response, string result)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(result) && IsValidJson(result))
                {
                    using var document = JsonDocument.Parse(result);
                    var root = document.RootElement;

                    if (
                        root.TryGetProperty("Data", out var data)
                        && data.TryGetProperty("Mesage", out _)
                        && data.TryGetProperty("Inner", out _)
                        && data.TryGetProperty("Trace", out _)
                    )
                    {
                        throw new ExternalApiException(
                            JsonSerializer.Deserialize<ExternalApiError>(result, _jsonOptions)!
                        );
                    }
                }

                if (TryDeserialize(result, out T? resultResponseWithError) && resultResponseWithError != null! && !IsAllPropertiesDefault(resultResponseWithError))
                {
                    return await Task.FromResult(resultResponseWithError);
                }

                throw new GenericException($"Error al procesar la solicitud: Status: {(int)response.StatusCode}, Description: {response.ReasonPhrase}");
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                return await Task.FromResult<T>(default!);
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)result;
            }

            return await Task.FromResult(JsonSerializer.Deserialize<T>(result, _jsonOptions)!);
        }
        // Fin refactorización/optimización por GitHub Copilot

        private static HttpContent GetContentAsync(ExternalApiClientRequest externalApiClientRequest, string bodyJson)
        {
            HttpContent? content = null;

            if (externalApiClientRequest.Files != null && externalApiClientRequest.Files.Count != 0)
            {
                var multipartContent = new MultipartFormDataContent();

                if (externalApiClientRequest.Body != null)
                {
                    var bodyDict = JsonSerializer.Deserialize<Dictionary<string, object>>(bodyJson, _jsonOptions);

                    foreach (var field in bodyDict!)
                    {
                        multipartContent.Add(new StringContent(field.Value?.ToString() ?? string.Empty), field.Key);
                    }
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

                content = multipartContent;
            }
            else if (externalApiClientRequest.Body != null)
            {
                if (externalApiClientRequest.IsFormUrlEncoded is null)
                {
                    content = new StringContent(
                        bodyJson,
                        Encoding.UTF8,
                        "application/json"
                    );
                }
                else
                {
                    var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(bodyJson, _jsonOptions);

                    content = new FormUrlEncodedContent(dict!);
                }
            }

            return content!;
        }

        private async Task CreateRequestLogAsync(
            ExternalApiClientRequest externalApiClientRequest,
            string result,
            string url,
            HttpResponseMessage response,
            HttpRequestMessage request,
            string bodyJson
        )
        {
            CreateRequest logRequest = new()
            {
                Request = externalApiClientRequest.BodyExternalRequest != null
                    ? JsonSerializer.Serialize(externalApiClientRequest.BodyExternalRequest, _jsonOptions)
                    : externalApiClientRequest.Body != null
                        ? TruncateLargePayload(bodyJson)
                        : string.Empty,
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
            string baseUrl = externalApiClientRequest.BaseUrlReconoser ?? _config.GetSection($"{externalApiClientRequest.Section}:{externalApiClientRequest.BaseUrl}")?.Value!;
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

        // Método generado por GitHub Copilot
        public static bool IsValidJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            try
            {
                using var doc = JsonDocument.Parse(input);

                return doc.RootElement.ValueKind == JsonValueKind.Object ||
                       doc.RootElement.ValueKind == JsonValueKind.Array;
            }
            catch (JsonException)
            {
                return false;
            }
        }
        // Fin código generado por GitHub Copilot

        private static bool TryDeserialize<T>(string json, out T? result)
        {
            result = default;

            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            try
            {
                result = JsonSerializer.Deserialize<T>(json, _jsonOptions);

                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsAllPropertiesDefault<T>(T obj)
        {
            if (obj == null)
                return true;

            foreach (var p in typeof(T).GetProperties())
            {
                var value = p.GetValue(obj);

                if (!p.PropertyType.IsValueType)
                {
                    if (value != null)
                        return false;
                }
                else
                {
                    var defaultValue = Activator.CreateInstance(p.PropertyType);
                    if (!Equals(value, defaultValue))
                        return false;
                }
            }

            return true;
        }
    }
}
