using Azure.Identity;
using Azure.Storage.Blobs;
using Nuget.LogService.Services;
using Process.API.Extensions;
using Process.API.Middleware;
using Process.Application;
using Process.Infrastructure;
using Process.Infrastructure.Data;
using Process.Infrastructure.Shared;
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

//BlobStorage para AuditLogs pesados
var blobContainer = new BlobContainerClient(
    builder.Configuration["ConnectionStrings:AzureBlobStorage"],
    "audit-logs"
);

await blobContainer.CreateIfNotExistsAsync();
builder.Services.AddSingleton(blobContainer);

// Configurar Serilog
Log.Logger = BlobLoggerService.CreateBlobLogger(
    builder.Configuration["ConnectionStrings:AzureBlobStorage"]!,
    builder.Configuration["AzureBlobStorageFolders:LogsContainer"]!,
    "Process"
);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.CustomAddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

// Dependency Injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<ExternalApiSettings>(
    builder.Configuration.GetSection("ExternalApi"));

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "ApiCentralLog",
    baseAddress: builder.Configuration.GetSection("ApiCentralLog:UrlBase").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "ApiCentralAdtp",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlAdtp").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "ApiCentralSso",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlSso").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlReconoser1",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlReconoser1").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlReconoser2",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlReconoser2").Value!
);


builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlAni",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlAni").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlMegvi",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlMegvi").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlIbeta1",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlIbeta1").Value!
);


builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlIbeta2",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlIbeta2").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlVeriFace",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlVeriFace").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlVerId",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlVerId").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlTokenJumio",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlTokenJumio").Value!
);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "BaseUrlAccountJumio",
    baseAddress: builder.Configuration.GetSection("ExternalApi:BaseUrlAccountJumio").Value!
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
await DatabaseMigrationManagementService.MigrationInitialisation(app);

// Manejo global de errores
app.UseMiddleware<ExecutionMiddleware>();

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.CustomMapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
