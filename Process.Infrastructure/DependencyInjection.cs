using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nuget.LogService.Services;
using Process.Application.Context;
using Process.Application.Interfaces;
using Process.Domain.Interfaces;
using Process.Domain.Parameters.Context;
using Process.Domain.Services;
using Process.Infrastructure.Data;
using Process.Infrastructure.Services;

namespace Process.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
            services.AddScoped<Domain.Repositories.IAgreementProcessRepository, Repositories.AgreementProcessRepository>();
            services.AddScoped<Domain.Repositories.IPersonalizationAgreementRepository, Repositories.PersonalizationAgreementRepository>();
            services.AddScoped<Domain.Repositories.IAgreementRepository, Repositories.AgreementRepository>();
            services.AddScoped<Domain.Repositories.ICitizenRepository, Repositories.CitizenRepository>();
            services.AddScoped<Domain.Repositories.IUserRepository, Repositories.UserRepository>();
            services.AddScoped<Domain.Repositories.IUserOkeyRepository, Repositories.UserOkeyRepository>();
            services.AddScoped<Domain.Repositories.IAgreementATDPRepository, Repositories.AgreementATDPRepository>();
            services.AddScoped<Domain.Repositories.IBiometricKeysTempRepository, Repositories.BiometricKeysTempRepository>();
            services.AddScoped<Domain.Repositories.ISedeRepository, Repositories.SedeRepository>();
            services.AddScoped<Domain.Repositories.IDocumentTypeRepository, Repositories.DocumentTypeRepository>();
            services.AddScoped<Domain.Repositories.IAgreementByUserRepository, Repositories.AgreementByUserRepository>();
            services.AddScoped<Domain.Repositories.IAgreementByGuidRepository, Repositories.AgreementByGuidRepository>();
            services.AddScoped<Domain.Repositories.IValidationProcessRepository, Repositories.ValidationProcessRepository>();
            services.AddScoped<Domain.Repositories.IValidationProcessExecutionRepository, Repositories.ValidationProcessExecutionRepository>();
            services.AddScoped<Domain.Repositories.IAgreementOkeyStudioRepository, Repositories.AgreementOkeyStudioRepository>();
            services.AddScoped<Domain.Repositories.IReasonRepository, Repositories.ReasonRepository>();
            services.AddScoped<Domain.Repositories.ICitizenBiometricsDocumentsRepository, Repositories.CitizenBiometricsDocumentsRepository>();
            services.AddScoped<Domain.Repositories.ICountryRepository, Repositories.CountryRepository>();
            services.AddScoped<Domain.Repositories.IPasswordRecoveryNotificationRepository, Repositories.PasswordRecoveryNotificationRepository>();
            services.AddScoped<Domain.Repositories.IPasswordRecoveryHistoryRepository, Repositories.PasswordRecoveryHistoryRepository>();
            services.AddScoped<Domain.Repositories.IRoleRepository, Repositories.RoleRepository>();
            services.AddScoped<Domain.Repositories.IRoleByUserRepository, Repositories.RoleByUserRepository>();
            services.AddScoped<Domain.Repositories.IRoleAgreementByUserRepository, Repositories.RoleAgreementByUserRepository>();
            services.AddScoped<Domain.Repositories.IRoleByAgreementRepository, Repositories.RoleByAgreementRepository>();
            services.AddScoped<Domain.Repositories.IClientRepository, Repositories.ClientRepository>();
            services.AddScoped<Domain.Repositories.IMenuRepository, Repositories.MenuRepository>();
            services.AddScoped<Domain.Repositories.IRoleMenuRepository, Repositories.RoleMenuRepository>();
            services.AddScoped<Domain.Repositories.IPermissionRepository, Repositories.PermissionRepository>();
            services.AddScoped<Domain.Repositories.IRolePermissionRepository, Repositories.RolePermissionRepository>();
            services.AddScoped<Domain.Repositories.IDocumentTypeCaptureRepository, Repositories.DocumentTypeCaptureRepository>();
            services.AddScoped<Domain.Repositories.IForensicReviewProcessRepository, Repositories.ForensicReviewProcessRepository>();
            services.AddScoped<Domain.Repositories.IStatusForensicRepository, Repositories.StatusForensicRepository>();
            services.AddScoped<Domain.Repositories.IStatusValidationRepository, Repositories.StatusValidationRepository>();
            services.AddScoped<Domain.Repositories.IValidationProcessDocumentsRepository, Repositories.ValidationProcessDocumentsRepository>();
            services.AddScoped<Domain.Repositories.IValidationProcessAuditLogsRepository, Repositories.ValidationProcessAuditLogsRepository>();
            services.AddScoped<Domain.Repositories.IValidationProcessScoresRepository, Repositories.ValidationProcessScoresRepository>();
            services.AddScoped<Domain.Repositories.IParametersAgreementRepository, Repositories.ParametersAgreementRepository>();
            services.AddScoped<Domain.Repositories.ITempProcessKeysRepository, Repositories.TempProcessKeysRepository>();
            services.AddScoped<Domain.Repositories.IMobileDeviceInfoRepository, Repositories.MobileDeviceInfoRepository>();
            services.AddScoped<Domain.Repositories.IValidationProcessDeviceInfoRepository, Repositories.ValidationProcessDeviceInfoRepository>();

            services.AddScoped<Domain.Repositories.ICountriesAndRegionsRepository, Repositories.CountriesAndRegionsRepository>();
            services.AddScoped<Domain.Repositories.IDocumentTypeByCountryRepository, Repositories.DocumentTypeByCountryRepository>();
            services.AddScoped<Domain.Repositories.IMonitoringDataDogImg, Repositories.MonitoringDataDog>();
            services.AddScoped<Domain.Services.ISigningCredentialService, SigningCredentialService>();
            services.AddScoped<Domain.Services.IBlobStorageService, BlobStorageService>();
            services.AddScoped<ReconoserContext>();
            services.AddTransient<Domain.Services.ISsoService, SsoService>();
            services.AddTransient<Domain.Services.IAuthService, AuthService>();
            services.AddTransient<Domain.Services.IExternalApiClientService, ExternalApiClientService>();
            services.AddTransient<Domain.Services.IReconoserApiService, ReconoserApiService>();
            services.AddTransient<Domain.Services.IAniApiService, AniApiService>();
            services.AddTransient<Domain.Services.ICompareFacesMegviApiService, CompareFacesMegviApiService>();
            services.AddTransient<Domain.Services.IIBetaApiService, IBetaApiService>();
            services.AddTransient<Domain.Services.IVeriFaceApiService, VeriFaceApiService>();
            services.AddTransient<Domain.Services.IVerIdService, VerIdService>();
            services.AddTransient<Domain.Services.IJumioService, JumioService>();
            services.AddTransient<Domain.Services.IConfigurationService, ConfigurationService>();
            services.AddTransient<Domain.Services.ITokenCacheService, TokenCacheService>();
            services.AddTransient<Domain.Services.IWhatsappService, WhatsappService>();
            services.AddTransient<Domain.Services.IRecaptchaServices, RecaptchaServices>();
            services.AddTransient<Domain.Services.IEncryptionKeyProviderService, EncryptionKeyProviderService>();
            services.AddTransient<Domain.Services.ITokenService, TokenService>();
            services.AddTransient<Domain.Services.IEncryptImageBlobService, EncryptImageBlobService>();
            services.AddTransient<IUnitOfWork, DapperUnitOfWork>(c => new DapperUnitOfWork(connectionString));
            services.AddDbContext<ContextSqlServerDB>(options => options.UseSqlServer(connectionString));
            services.AddSingleton<SQLServerConnectionFactory>();
            var logAPIBaseURL = configuration.GetRequiredSection("ApiCentralLog:UrlBase").Value;
            var logAPIToken = configuration.GetRequiredSection("ApiCentralLog:Token").Value;
            // Inyectar LogServices con el ILogger correctamente
            services.AddScoped<ILogServices>(c =>
            {
                var logger = c.GetRequiredService<ILogger<LogServices>>();
                var clientFactory = c.GetRequiredService<IHttpClientFactory>();
                return new LogServices(logAPIBaseURL!, logAPIToken!, logger, clientFactory);
            });
            services.AddTransient<IAtdpApiClient, RefitAtdpApiClient>();

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = NormalizeBackslashes(configuration.GetConnectionString("OKeyConnection")!);
                options.SchemaName = "dbo";
                options.TableName = "TokenCache";
            });

            services.AddSingleton<IAuditQueueService, AuditQueueService>();
            services.AddHostedService<AuditWorkerService>();

            return services;
        }

        private static string NormalizeBackslashes(string value)
        {
            while (value.Contains(@"\\"))
            {
                value = value.Replace(@"\\", @"\");
            }

            return value;
        }
    }
}
