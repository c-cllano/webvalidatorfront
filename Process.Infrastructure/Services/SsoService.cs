using Microsoft.Extensions.Configuration;
using Nuget.LogService.Models;
using Nuget.LogService.Services;
using Process.Domain.Entities;
using Process.Domain.Exceptions;
using Process.Domain.Parameters.Sso.Authentications;
using Process.Domain.Parameters.Sso.User; 
using Process.Domain.Services;
using Process.Domain.Utilities;
using System.Net.Http.Json;
using System.Text.Json;

namespace Process.Infrastructure.Services
{
    public class SsoService(
        IHttpClientFactory _clientFactory,
        IConfiguration config,
        ILogServices logService
    ) : ISsoService
    {
        private readonly HttpClient _client = _clientFactory.CreateClient("IgnoreSSL");
        private readonly string _url = config.GetSection("ExternalApi:BaseUrlSso")?.Value ?? string.Empty; 
        private readonly ILogServices _logService = logService;

        public async Task<SsoServiceResult<TokenResponse>> GetToken(Token getToken)
        {
            string url = $"{_url}api/Authentications/Token";

            HttpRequestMessage request = new(HttpMethod.Post, url)
            {
                Content = BuildHttpContent(getToken)
            };
            HttpResponseMessage response = await _client.SendAsync(request);

            ThreadExecutionHelper.ThreadExecution(() => CreateRequestLogAsync(getToken, url, response, HttpMethod.Post));

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = (int)response.StatusCode;
                string error = await response.Content.ReadAsStringAsync();
                return SsoServiceResult<TokenResponse>.Fail(error, statusCode);
            }
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>() ?? new TokenResponse();
            return SsoServiceResult<TokenResponse>.Ok(tokenResponse);
        }

        private static FormUrlEncodedContent BuildHttpContent(Token token)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new("grant_type", token.GrantType),
                new("client_id", token.ClientId)
            };

            if (token.GrantType.Equals("refresh_token", StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.Add(new("refresh_token", token.RefreshToken));
            }
            else
            {
                parameters.Add(new("username", token.UserName));
                parameters.Add(new("password", token.Password));
            }

            if (!string.IsNullOrEmpty(token.ClientSecret))
            {
                parameters.Add(new("client_secret", token.ClientSecret));
            }

            parameters.Add(new("client_type", token.ClientType ?? "API"));

            return new FormUrlEncodedContent(parameters);
        }

        public async Task<int> ForgotPassword(ForgotPassword forgotPassword)
        {
            var response = await PostAsync("api/Account/ForgotPassword", forgotPassword);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task<object?> ResetPassword(ResetPassword resetPassword)
        {
            return await PostAsync("api/Account/ResetPassword", resetPassword);
        }

        public async Task<object?> ChangePassword(ChangePassword changePassword)
        {
            return await PostAsync("api/Account/ChangePassword", changePassword);
        }

        public async Task<object?> CreateClient(CreateClient createClient)
        {
            return await PostAsync("api/client/Create_client", createClient);
        }

        public async Task<object?> ApplicationCreate(ApplicationCreate applicationCreate)
        {
            return await PostAsync("api/Configuration/Application/Create", applicationCreate);
        }

        public async Task<UserSsoCreated?> Register(CreateUser createUser)
        {
            var userCreated = await PostAsync("api/User/Register", createUser);
            return  await userCreated.Content.ReadFromJsonAsync<UserSsoCreated>();
        }

        public async Task<UserSso> GetUser(int userId)
        {
            string url = $"{_url}api/User/{userId}";

            HttpRequestMessage request = new(HttpMethod.Get, url);
            HttpResponseMessage response = await _client.SendAsync(request);

            ThreadExecutionHelper.ThreadExecution(() => CreateRequestLogAsync(null, url, response, HttpMethod.Get));

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = response.StatusCode;
                string error = await response.Content.ReadAsStringAsync();
                throw new BusinessException(error, (int)statusCode);
            }

            UserSso? userSso = await response.Content.ReadFromJsonAsync<UserSso>();
            return userSso!;            
        }

        public async Task<SsoServiceResult<List<UserSso>>> GetUsers(UserQueryParameters user)
        {

            List<string> queryParams = [];
            if (user.PageNumber.HasValue) queryParams.Add($"pageNumber={user.PageNumber.Value}");
            if (user.PageSize.HasValue) queryParams.Add($"pageSize={user.PageSize.Value}");
            if (!string.IsNullOrEmpty(user.Email)) queryParams.Add($"email={Uri.EscapeDataString(user.Email)}");
            if (!string.IsNullOrEmpty(user.FirstName)) queryParams.Add($"firstName={Uri.EscapeDataString(user.FirstName)}");
            if (!string.IsNullOrEmpty(user.LastName)) queryParams.Add($"lastName={Uri.EscapeDataString(user.LastName)}");
            if (!string.IsNullOrEmpty(user.SecondName)) queryParams.Add($"secondName={Uri.EscapeDataString(user.SecondName)}");
            if (!string.IsNullOrEmpty(user.SecondLastName)) queryParams.Add($"secondLastName={Uri.EscapeDataString(user.SecondLastName)}");
            string url = $"{_url}api/User/GetUsersByClient/{user.ClientId}";

            if (queryParams.Count != 0)
                url += "?" + string.Join("&", queryParams);

            HttpResponseMessage response = await _client.GetAsync(url);

            ThreadExecutionHelper.ThreadExecution(() => CreateRequestLogAsync(null, url, response, HttpMethod.Get));

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = (int)response.StatusCode;
                string error = await response.Content.ReadAsStringAsync();
                return SsoServiceResult<List<UserSso>>.Fail(error, statusCode);
            }

            var users = await response.Content.ReadFromJsonAsync<List<UserSso>>() ?? new List<UserSso>();
            int contador = 1;
            foreach (var userList in users)
            {
                userList.UserIdRow = contador++;
            }

            return SsoServiceResult<List<UserSso>>.Ok(users);
        }

        public async Task<object?> UpdateUser(int userId, UpdateUser updateUser)
        {
            string url = $"{_url}api/User/{userId}";
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, updateUser);

            ThreadExecutionHelper.ThreadExecution(() => CreateRequestLogAsync(updateUser, url, response, HttpMethod.Put));

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = response.StatusCode;
                string error = await response.Content.ReadAsStringAsync();
                throw new BusinessException(error, (int)statusCode);
            }

            return response.StatusCode;
        }

        public async Task<object?> DeleteUser(int userId)
        {
            string url = $"{_url}api/User/{userId}";
            HttpResponseMessage response = await _client.DeleteAsync(url);

            ThreadExecutionHelper.ThreadExecution(() => CreateRequestLogAsync(null, url, response, HttpMethod.Delete));

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = response.StatusCode;
                string error = await response.Content.ReadAsStringAsync();
                throw new BusinessException(error, (int)statusCode);
            }

            return response.StatusCode;
        }


        private async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T model)
        {
            string url = $"{_url}{endpoint}";
            var response = await _client.PostAsJsonAsync(url, model);

            ThreadExecutionHelper.ThreadExecution(() => CreateRequestLogAsync(model, url, response, HttpMethod.Post));

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = response.StatusCode;
                string error = await response.Content.ReadAsStringAsync();
                throw new BusinessException(error, (int)statusCode);
            }
            return response;
        }

        private async Task CreateRequestLogAsync(
            object? body,
            string url,
            HttpResponseMessage response,
            HttpMethod method
        )
        {
            string requestString = string.Empty;
            string result = await response.Content.ReadAsStringAsync();

            if (body != null)
            {
                requestString = JsonSerializer.Serialize(body);
            }

            CreateRequest logRequest = new()
            {
                Request = requestString,
                Response = result,
                Type = method.Method,
                Endpoint = url,
                Status = (int)response.StatusCode,
                StartingTime = DateTime.UtcNow.AddHours(-5),
                FinalTime = DateTime.UtcNow.AddHours(-5),
                Component = "SsoService",
                TransactionID = Guid.NewGuid().ToString(),
                Machine = Environment.MachineName,
                UserID = Environment.UserName,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _logService.CreateRequestAsync(logRequest);
        }
    }
}
