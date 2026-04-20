using Azure.Identity;
using DrawFlowProcess.API.Extensions;
using DrawFlowProcess.API.Middleware;
using DrawFlowProcess.Application;
using DrawFlowProcess.Application.LogServices.CreateError;
using DrawFlowProcess.Infrastructure;
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
    "DrawFlowProcess"
);

builder.Host.UseSerilog(); // Usar Serilog como el proveedor de logs


// Add services to the container.

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

builder.Services.CustomAddOpenApi();

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

//DependencyInjection
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "ApiCentralLog",
    baseAddress: builder.Configuration.GetSection("ApiCentralLog:UrlBase").Value!
);

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseMiddleware<UserAgentMiddleware>();

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
                Component = "DrawflosProcess.API",
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.CustomMapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();