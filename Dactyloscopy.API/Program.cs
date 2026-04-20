using Azure.Identity;
using Dactyloscopy.API.Extensions;
using Dactyloscopy.API.Response;
using Dactyloscopy.Application;
using Dactyloscopy.Application.LogServices.CreateError;
using Dactyloscopy.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Nuget.LogService.Services;
using Serilog;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

// KeyVault
var keyVaultName = builder.Configuration["KeyVault:Name"];
var connectionKeyVault = bool.Parse(builder.Configuration["KeyVault:ConnectionKeyVault"]!);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

if (!string.IsNullOrWhiteSpace(keyVaultName) && connectionKeyVault)
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}

// Configurar Serilog
Log.Logger = BlobLoggerService.CreateBlobLogger(
    builder.Configuration["ConnectionStrings:AzureBlobStorage"]!,
    builder.Configuration["AzureBlobStorageFolders:LogsContainer"]!,
    "Dactyloscopy"
);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.CustomAddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

// Dependency Injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "ApiCentralLog",
    baseAddress: builder.Configuration.GetSection("ApiCentralLog:UrlBase").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlDactyloscopy",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlDactyloscopy").Value!
);

builder.Services.AddHttpClient("IgnoreSSL")
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
        });

var app = builder.Build();
app.MapHealthChecks("/health");

// Manejo global de errores
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var mediator = context.RequestServices.GetRequiredService<IMediator>();

        if (exceptionHandlerPathFeature?.Error is Dactyloscopy.Domain.Exceptions.BusinessException businessEx)
        {
            await mediator.Send(new CreateErrorCommand()
            {
                SeverityID = 5,
                Description = businessEx.Error,
                Component = "Dactyloscopy.API",
                Date = DateTime.UtcNow.AddHours(-5)
            });

            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                Message = businessEx.Error,
                Code = "BUSINESS_ERROR",
                Status = businessEx.StatusCode
            });
        }
        else if (exceptionHandlerPathFeature?.Error is Dactyloscopy.Domain.Exceptions.ExternalApiException externalApiEx)
        {
            await mediator.Send(new CreateErrorCommand()
            {
                SeverityID = 5,
                Description = externalApiEx.MessageError!,
                Component = "Dactyloscopy.API",
                Date = DateTime.UtcNow.AddHours(-5)
            });

            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                Message = externalApiEx.MessageError!,
                Code = "EXTERNAL_API_ERROR",
                Status = externalApiEx.Code!.Value
            });
        }
        else if (exceptionHandlerPathFeature?.Error is not Dactyloscopy.Domain.Exceptions.BusinessException)
        {

            await mediator.Send(new CreateErrorCommand()
            {
                SeverityID = 5,
                Description = exceptionHandlerPathFeature?.Error.ToString()!,
                Component = "Dactyloscopy.API",
                Date = DateTime.UtcNow.AddHours(-5)
            });
            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                Message = exceptionHandlerPathFeature?.Error?.Message ?? "Ocurrió un error inesperado. Por favor, intente nuevamente.",
                Code = "INTERNAL_ERROR",
                Status = 500
            });
        }
    });
});

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.CustomMapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

