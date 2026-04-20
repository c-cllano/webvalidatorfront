using ApiGateways.API.Core.Handlers;
using ApiGateways.API.Extensions;
using ApiGateways.Aplication;
using ApiGateways.Infrastructure;
using ApiGateways.Infrastructure.Utils;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Nuget.LogService.Services;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Polly;
using Polly.CircuitBreaker;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// KeyVault
var keyVaultName = builder.Configuration["KeyVault:Name"];
var connectionKeyVault = bool.Parse(builder.Configuration["KeyVault:ConnectionKeyVault"]!);

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
    "ApiGateways"
);

builder.Host.UseSerilog(); // Usar Serilog como el proveedor de logs

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

string? ocelotJson;
using (var reader = new StreamReader("ocelot.json"))
{
    ocelotJson = await reader.ReadToEndAsync();
}
Dictionary<string, string> envVariables = new()
{
    { "PROCESS_HOST", Environment.GetEnvironmentVariable("PROCESS_HOST") ?? builder.Configuration.GetValue<string>("PROCESS_HOST")! },
    { "PROCESS_PORT", Environment.GetEnvironmentVariable("PROCESS_PORT") ?? builder.Configuration.GetValue<string>("PROCESS_PORT")! },
    { "UICONFIGURATION_HOST", Environment.GetEnvironmentVariable("UICONFIGURATION_HOST") ?? builder.Configuration.GetValue<string>("UICONFIGURATION_HOST")! },
    { "UICONFIGURATION_PORT", Environment.GetEnvironmentVariable("UICONFIGURATION_PORT") ?? builder.Configuration.GetValue<string>("UICONFIGURATION_PORT")! },
    { "DACTYLOSCOPY_HOST", Environment.GetEnvironmentVariable("DACTYLOSCOPY_HOST") ?? builder.Configuration.GetValue<string>("DACTYLOSCOPY_HOST")! },
    { "DACTYLOSCOPY_PORT", Environment.GetEnvironmentVariable("DACTYLOSCOPY_PORT") ?? builder.Configuration.GetValue<string>("DACTYLOSCOPY_PORT")! },
    { "DRAWFLOWPROCESS_HOST", Environment.GetEnvironmentVariable("DRAWFLOWPROCESS_HOST") ?? builder.Configuration.GetValue<string>("DRAWFLOWPROCESS_HOST")! },
    { "DRAWFLOWPROCESS_PORT", Environment.GetEnvironmentVariable("DRAWFLOWPROCESS_PORT") ?? builder.Configuration.GetValue<string>("DRAWFLOWPROCESS_PORT")! },
    { "DRAWFLOWCONFIGURATION_HOST", Environment.GetEnvironmentVariable("DRAWFLOWCONFIGURATION_HOST") ?? builder.Configuration.GetValue<string>("DRAWFLOWCONFIGURATION_HOST")! },
    { "DRAWFLOWCONFIGURATION_PORT", Environment.GetEnvironmentVariable("DRAWFLOWCONFIGURATION_PORT") ?? builder.Configuration.GetValue<string>("DRAWFLOWCONFIGURATION_PORT")! },
    { "UITEMPLATE_HOST", Environment.GetEnvironmentVariable("UITEMPLATE_HOST") ?? builder.Configuration.GetValue<string>("UITEMPLATE_HOST")! },
    { "UITEMPLATE_PORT", Environment.GetEnvironmentVariable("UITEMPLATE_PORT") ?? builder.Configuration.GetValue<string>("UITEMPLATE_PORT")! },
    { "DIGITALSIGNATURETEMPLATE_HOST", Environment.GetEnvironmentVariable("DIGITALSIGNATURETEMPLATE_HOST") ?? builder.Configuration.GetValue<string>("DIGITALSIGNATURETEMPLATE_HOST")! },
    { "DIGITALSIGNATURETEMPLATE_PORT", Environment.GetEnvironmentVariable("DIGITALSIGNATURETEMPLATE_PORT") ?? builder.Configuration.GetValue<string>("DIGITALSIGNATURETEMPLATE_PORT")! },
    { "BASE_URL", Environment.GetEnvironmentVariable("BASE_URL") ?? builder.Configuration.GetValue<string>("BASE_URL")! },
    { "RECONOSERID_LOGS_HOST", Environment.GetEnvironmentVariable("RECONOSERID_LOGS_HOST") ?? builder.Configuration.GetValue<string>("RECONOSERID_LOGS_HOST")! },
    { "RECONOSERID_LOGS_PORT", Environment.GetEnvironmentVariable("RECONOSERID_LOGS_PORT") ?? builder.Configuration.GetValue<string>("RECONOSERID_LOGS_PORT")! }
};
ocelotJson = envVariables.Aggregate(ocelotJson, (json, entry) =>
{
    if (entry.Key.EndsWith("_PORT"))
    {
        return json.Replace($"\"{entry.Key}\"", entry.Value);
    }
    else
    {
        return json.Replace($"\"{entry.Key}\"", $"\"{entry.Value}\"");
    }
});
string? tempOcelotPath = Path.Combine(AppContext.BaseDirectory, "ocelot.generated.json");
await File.WriteAllTextAsync(tempOcelotPath, ocelotJson);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile(tempOcelotPath, optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Services.AddSingleton<IReadOnlyDictionary<string, AsyncCircuitBreakerPolicy<HttpResponseMessage>>>(provider =>
{
    static AsyncCircuitBreakerPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>() // Maneja excepciones de red
            .OrResult(response => !response.IsSuccessStatusCode) // Maneja respuestas no exitosas
            .AdvancedCircuitBreakerAsync(
                failureThreshold: 0.7, // Porcentaje de fallos para abrir el circuito (70%)
                samplingDuration: TimeSpan.FromSeconds(60), // Ventana de observación de 60s
                minimumThroughput: 10, // Debe haber al menos 10 solicitudes en la ventana
                durationOfBreak: TimeSpan.FromSeconds(30));// Mantiene el circuito abierto 30s antes de intentarlo de nuevo
    }
    return new Dictionary<string, AsyncCircuitBreakerPolicy<HttpResponseMessage>>
    {
        { Microservice.ProcessAPI, CreateCircuitBreakerPolicy() },
        { Microservice.UIConfigurationAPI, CreateCircuitBreakerPolicy() },
        { Microservice.DactyloscopyAPI, CreateCircuitBreakerPolicy() },
        { Microservice.DrawFlowProcessAPI, CreateCircuitBreakerPolicy() },
        { Microservice.DrawFlowConfigurationAPI, CreateCircuitBreakerPolicy() },
        { Microservice.UITemplateAPI, CreateCircuitBreakerPolicy() },
        { Microservice.DigitalSignatureAPI, CreateCircuitBreakerPolicy() }
    };
});



builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration.GetSection("OpenIddict:Base").Value!;
        options.RequireHttpsMetadata = true;
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
              HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetSection("OpenIddict:Base").Value!,
            ValidateAudience = true,
            ValidAudience = "58b6dc96-aa9f-4db8-bf51-897cbefd4f83",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"❌ Auth failed: {ctx.Exception}");
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine($"✅ Token válido para: {ctx.Principal?.Identity?.Name}");
                return Task.CompletedTask;
            }
        };
    });






builder.Services.AddTransient<EncodingDelegatingHandler>();
builder.Services.AddHttpClient("MyHttpClient").AddHttpMessageHandler<EncodingDelegatingHandler>();


builder.Services
    .AddOcelot(builder.Configuration)
    .AddDelegatingHandler<EncodingDelegatingHandler>(true)
    .AddDelegatingHandler<IgnoreSslHandler>(true)
    .AddCacheManager(x =>
    {
        x.WithDictionaryHandle();
    });


builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    }
    else
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    }
});

//DependencyInjection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddNamedHttpClientWithDefaultResilience(
    clientName: "ApiCentralLog",
    baseAddress: builder.Configuration.GetSection("ApiCentralLog:UrlBase").Value!
);

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Inicio código generado por GitHub Copilot
// Middleware para asegurar que X-Forwarded-For esté presente para rate limiting de Ocelot
app.Use(async (context, next) =>
{
    if (!context.Request.Headers.ContainsKey("X-Forwarded-For"))
    {
        string clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        context.Request.Headers.Append("X-Forwarded-For", clientIp);
    }
    await next();
});
// Fin código generado por GitHub Copilot

app.MapHealthChecks("/health");
app.UseRouting();


app.Use(async (context, next) =>
{
    context.Response.Headers.Clear();
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Append("X-UA-Compatible", "IE=10");
    context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000");
    context.Response.Headers.Append("Content-Security-Policy", "default-src http: https:; img-src http: https: data:; style-src http: https: 'unsafe-inline'; script-src http: https: 'unsafe-inline'; frame-ancestors 'self';");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    context.Response.Headers.Append("Permissions-Policy", "geolocation=(self), microphone=(self), camera=(self)");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");
    context.Response.Headers.Append("Expect-CT", "max-age=86400, enforce");
    context.Response.Headers.Append("Cache-Control", "no-store");
    await next();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.UseOcelot().Wait();

await app.RunAsync();

