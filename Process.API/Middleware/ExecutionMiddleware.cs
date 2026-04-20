using MediatR;
using Process.API.Attributes;
using Process.API.Response;
using Process.Application.LogServices.CreateError;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Process.API.Middleware
{
    public class ExecutionMiddleware(
        RequestDelegate next,
        IAuditQueueService auditQueue)
    {
        private readonly RequestDelegate _next = next;
        private readonly IAuditQueueService _auditQueue = auditQueue;

        private const string CLAIM_SUB = "sub";
        private const string CLAIM_CLIENT_TYPE = "client_type";
        private const string HEADER_AUTHORIZATION = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";
        private const string API_PERMISSION = "Usuario consola API";

        private const string CLIENT_TYPE_WEB = "WEB";
        private const string CLIENT_TYPE_API = "API";

        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true
        };

        public async Task InvokeAsync(HttpContext context)
        {
            if (!await CanAccess(context))
                return;

            if (!CanAudit(context))
                await ExecutionWithoutAudit(context);
            else
                await ExecutionWithAudit(context);
        }


        private async Task<bool> CanAccess(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null || !RequiresAccessValidation(endpoint))
                return true;

            var tokenData = GetTokenData(context);
            if (tokenData == null)
                return false;

            return await ValidateByClientType(context, tokenData.Value.UserId, tokenData.Value.ClientType);
        }

        private async Task<bool> ValidateByClientType(HttpContext context, long userId, string clientType)
        {
            return clientType.ToUpperInvariant() switch
            {
                CLIENT_TYPE_WEB => await ValidateWebRequest(context, userId),
                CLIENT_TYPE_API => await ValidateApiRequest(context, userId),
                _ => Deny(context)
            };
        }

        private static async Task<bool> ValidateWebRequest(HttpContext context, long userId)
        {
            var menus = await GetMenusByUser(context, userId);

            var hasApiPermission = menus.Any(m =>
                m.Title == API_PERMISSION);

            if (hasApiPermission)
                return Deny(context);

            return true;
        }


        private async Task<bool> ValidateApiRequest(HttpContext context, long userId)
        {
            var menus = await GetMenusByUser(context, userId);

            var hasApiPermission = menus.Any(m =>
                m.Title == API_PERMISSION);

            if (!hasApiPermission)
                return Deny(context);

            return true;
        }

        private static (long UserId, string ClientType)? GetTokenData(HttpContext context)
        {
            var authHeader = context.Request.Headers[HEADER_AUTHORIZATION]
                .FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return null;
            }

            var token = authHeader.StartsWith(BEARER_PREFIX)
                ? authHeader[BEARER_PREFIX.Length..].Trim()
                : authHeader;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var subClaim = jwt.Claims
                .FirstOrDefault(c => c.Type == CLAIM_SUB)?.Value;

            var clientType = jwt.Claims
                .FirstOrDefault(c => c.Type == CLAIM_CLIENT_TYPE)?.Value;

            if (string.IsNullOrEmpty(subClaim) ||
                !long.TryParse(subClaim, out long userId) ||
                string.IsNullOrEmpty(clientType))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return null;
            }

            return (userId, clientType.ToUpper());
        }

        private static bool Deny(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return false;
        }

        private static bool RequiresAccessValidation(Endpoint endpoint)
        {
            return endpoint.Metadata
                .GetMetadata<AccessUriPermissionAttribute>() != null;
        }

        private static async Task<List<Menu>> GetMenusByUser(HttpContext context, long userId)
        {
            var rolesService = context.RequestServices
                .GetRequiredService<IRolePermissionRepository>();

            var roles = await rolesService.GetRoleByUser(userId, null);

            var menuService = context.RequestServices
                .GetRequiredService<IMenuRepository>();

            var allMenus = new List<Menu>();

            foreach (var role in roles)
            {
                var menusByRole = await menuService
                    .GetMenusByRoleAsync(role.RoleId);

                allMenus.AddRange(menusByRole);
            }

            return allMenus
                .GroupBy(m => m.MenuId)
                .Select(g => g.First())
                .ToList();
        }


        private static bool CanAudit(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
                return false;

            return endpoint.Metadata.GetMetadata<AuditLogAttribute>() != null;
        }

        private async Task ExecutionWithoutAudit(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task ExecutionWithAudit(HttpContext context)
        {
            string requestPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");

            context.Request.EnableBuffering();

            // Refactor generado por GitHub Copilot
            if (context.Request.HasFormContentType)
            {
                await SaveFormDataAsJson(context.Request, requestPath);
            }
            else
            {
                using var fs = new FileStream(requestPath, FileMode.Create);
                await context.Request.Body.CopyToAsync(fs);
            }
            // Fin código generado por GitHub Copilot

            context.Request.Body.Position = 0;

            var originalResponseBody = context.Response.Body;
            using var ms = new MemoryStream();
            context.Response.Body = ms;

            var start = DateTime.UtcNow;
            string errorException = null!;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                errorException = ex.ToJson();
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                var end = DateTime.UtcNow;

                ms.Position = 0;
                var responseBody = await new StreamReader(ms).ReadToEndAsync();
                ms.Position = 0;
                await ms.CopyToAsync(originalResponseBody);

                var auditLog = new ValidationProcessAuditLogs
                {
                    ValidationProcessId = null,
                    Url = GetFullUrl(context.Request),
                    Method = context.Request.Method,
                    RequestBody = requestPath,
                    ResponseBody = Truncate(responseBody),
                    StatusCode = context.Response.StatusCode,
                    DurationMs = (decimal)(end - start).TotalMilliseconds,
                    Exception = errorException
                };

                await _auditQueue.QueueAuditLog(auditLog);
            }
        }

        // Inicio código generado por GitHub Copilot
        // Método generado por GitHub Copilot
        private static async Task SaveFormDataAsJson(HttpRequest request, string outputPath)
        {
            try
            {
                var form = await request.ReadFormAsync();
                var jsonDict = new Dictionary<string, object>();

                foreach (var field in form)
                {
                    var value = field.Value.FirstOrDefault();

                    if (int.TryParse(value, out var intVal))
                        jsonDict[field.Key] = intVal;
                    else if (bool.TryParse(value, out var boolVal))
                        jsonDict[field.Key] = boolVal;
                    else
                        jsonDict[field.Key] = value ?? string.Empty;
                }

                if (form.Files.Any())
                {
                    var filesMetadata = form.Files.Select(f => new
                    {
                        f.Name,
                        f.FileName,
                        f.ContentType,
                        SizeBytes = f.Length
                    });

                    jsonDict["_files"] = filesMetadata;
                }

                var json = JsonSerializer.Serialize(jsonDict, jsonOptions);

                await File.WriteAllTextAsync(outputPath, json);
            }
            catch (Exception ex)
            {
                await File.WriteAllTextAsync(outputPath, JsonSerializer.Serialize(new
                {
                    error = "Error al leer form-data",
                    detail = ex.Message
                }));
            }
        }
        // Fin código generado por GitHub Copilot

        private static string Truncate(string text, int max = 200_000)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.Length <= max ? text : text[..max];
        }

        private static string GetFullUrl(HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var mediator = context.RequestServices.GetRequiredService<IMediator>();

            if (ex is Process.Domain.Exceptions.BusinessException businessEx)
            {
                await mediator.Send(new CreateErrorCommand()
                {
                    SeverityID = 5,
                    Description = businessEx.Error,
                    Component = "Process.API",
                    Date = DateTime.UtcNow.AddHours(-5)
                });

                await context.Response.WriteAsJsonAsync(new ApiErrorResponse
                {
                    Message = businessEx.Error,
                    Code = "BUSINESS_ERROR",
                    Status = businessEx.StatusCode
                });
            }
            else if (ex is Process.Domain.Exceptions.ExternalApiException externalApiEx)
            {
                await mediator.Send(new CreateErrorCommand()
                {
                    SeverityID = 5,
                    Description = externalApiEx.MessageError!,
                    Component = "Process.API",
                    Date = DateTime.UtcNow.AddHours(-5)
                });

                await context.Response.WriteAsJsonAsync(new ApiErrorResponse
                {
                    Message = externalApiEx.MessageError!,
                    Code = "EXTERNAL_API_ERROR",
                    Status = externalApiEx.Code!.Value
                });
            }
            else if (ex is not Process.Domain.Exceptions.BusinessException)
            {
                await mediator.Send(new CreateErrorCommand()
                {
                    SeverityID = 5,
                    Description = ex.InnerException?.Message ?? ex.Message,
                    Component = "Process.API",
                    Date = DateTime.UtcNow.AddHours(-5)
                });

                await context.Response.WriteAsJsonAsync(new ApiErrorResponse
                {
                    Message = ex.Message ?? "Ocurrió un error inesperado. Por favor, intente nuevamente.",
                    Code = "INTERNAL_ERROR",
                    Status = 500
                });
            }
        }



    }
}