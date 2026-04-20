using Dactyloscopy.Domain.Exceptions;
using Dactyloscopy.Domain.Parameters.ExternalApiClientParameters;
using Dactyloscopy.Domain.Services;
using Dactyloscopy.Domain.Utilities;
using Microsoft.Extensions.Configuration;
using Nuget.LogService.Models;
using Nuget.LogService.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Dactyloscopy.Infrastructure.Services
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

        private async Task<T> SendAsync<T>(
            ExternalApiClientRequest externalApiClientRequest,
            HttpMethod method
        )
        {
            HttpClient client = _clientFactory.CreateClient("IgnoreSSL");
            string url = GetApiUrl(externalApiClientRequest, method);

            HttpContent? content = GetContentAsync(externalApiClientRequest);

            HttpRequestMessage request = new(method, url)
            {
                Content = content
            };

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

            HttpResponseMessage response = await client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();

            ThreadExecutionHelper.ThreadExecution(() => CreateRequestLogAsync(externalApiClientRequest, result, url, response, request));

            if (!response.IsSuccessStatusCode)
            {
                if (TryDeserialize(result, out T? resultResponseWithError) && resultResponseWithError != null)
                {
                    return resultResponseWithError;
                }

                if (TryDeserialize(result, out ExternalApiError? resultError) && resultError != null)
                {
                    throw new ExternalApiException(resultError);
                }

                throw new IntegrationException($"Error al procesar la solicitud: Status: {(int)response.StatusCode}, Description: {response.ReasonPhrase}");
            }

            return JsonSerializer.Deserialize<T>(result)!;
        }

        private static HttpContent GetContentAsync(ExternalApiClientRequest externalApiClientRequest)
        {
            HttpContent? content = null;

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

                content = multipartContent;
            }
            else if (externalApiClientRequest.Body != null)
            {
                if (externalApiClientRequest.IsFormUrlEncoded is null)
                {
                    content = new StringContent(
                        JsonSerializer.Serialize(externalApiClientRequest.Body),
                        Encoding.UTF8,
                        "application/json"
                    );
                }
                else
                {
                    var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(
                        JsonSerializer.Serialize(externalApiClientRequest.Body)
                    );

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
