using Azure.Identity;
using DigitalSignature.API.Extensions;
using DigitalSignature.API.Response;
using DigitalSignature.Application;
using DigitalSignature.Application.LogServices.CreateError;
using DigitalSignature.Infrastructure;
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
    "DigitalSignature"
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
    clientName: "BaseUrlEmissionDocument",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlEmissionDocument").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlSignature",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlSignature").Value!
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

        if (exceptionHandlerPathFeature?.Error is DigitalSignature.Domain.Exceptions.BusinessException businessEx)
        {
            await mediator.Send(new CreateErrorCommand()
            {
                SeverityID = 5,
                Description = businessEx.Error,
                Component = "DigitalSignature.API",
                Date = DateTime.UtcNow.AddHours(-5)
            });

            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                Message = businessEx.Error,
                Code = "BUSINESS_ERROR",
                Status = businessEx.StatusCode
            });
        }
        else if (exceptionHandlerPathFeature?.Error is DigitalSignature.Domain.Exceptions.ExternalApiException externalApiEx)
        {
            await mediator.Send(new CreateErrorCommand()
            {
                SeverityID = 5,
                Description = externalApiEx.MessageError!,
                Component = "DigitalSignature.API",
                Date = DateTime.UtcNow.AddHours(-5)
            });

            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                Message = externalApiEx.MessageError!,
                Code = "EXTERNAL_API_ERROR",
                Status = externalApiEx.Code!.Value
            });
        }
        else if (exceptionHandlerPathFeature?.Error is not DigitalSignature.Domain.Exceptions.BusinessException)
        {

            await mediator.Send(new CreateErrorCommand()
            {
                SeverityID = 5,
                Description = exceptionHandlerPathFeature?.Error.ToString()!,
                Component = "DigitalSignature.API",
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
