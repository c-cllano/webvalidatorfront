using Azure.Identity;
using DrawFlowConfiguration.API.Extensions;
using DrawFlowConfiguration.Application;
using DrawFlowConfiguration.Application.LogServices.CreateError;
using DrawFlowConfiguration.Infrastructure;
using DrawFlowConfiguration.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Nuget.LogService.Services;
using Serilog;

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
    "DrawFlowConfiguration"
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

var app = builder.Build();
app.MapHealthChecks("/health");
await DatabaseMigrationManagementService.MigrationInitialisation(app);

// Manejo global de errores
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error != null)
        {
            var mediator = context.RequestServices.GetRequiredService<IMediator>();

            await mediator.Send(new CreateErrorCommand()
            {
                SeverityID = 5,
                Description = exceptionHandlerPathFeature.Error.ToString(),
                Component = "DrawFlowConfiguration.API",
                Date = DateTime.UtcNow.AddHours(-5)
            });

            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Ocurrió un error inesperado. Por favor, intente nuevamente.",
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

