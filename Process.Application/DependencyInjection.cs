using Microsoft.Extensions.DependencyInjection;
using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.UseCases.ServicesUseCases;
using System.Reflection;

namespace Process.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            var assembly = Assembly.GetExecutingAssembly();

            var typesWithAntiSpoofingAttribute = assembly.GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .Where(t => t.GetCustomAttribute<AntiSpoofingAttribute>() != null)
                .ToList();

            foreach (var type in typesWithAntiSpoofingAttribute)
            {
                services.AddScoped(type);
            }

            var typesWithPlatformAttribute = assembly.GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .Where(t => t.GetCustomAttribute<PlatformConnectionAttribute>() != null)
                .ToList();

            foreach (var type in typesWithPlatformAttribute)
            {
                services.AddScoped(type);
            }

            services.AddScoped(typeof(IAntiSpoofingStrategyService<>), typeof(AntiSpoofingStrategyService<>));
            services.AddScoped(typeof(IPlatformConnectionStrategyService<>), typeof(PlatformConnectionStrategyService<>));
            services.AddScoped<ICompareFacesService, CompareFacesOkeyService>();
            services.AddScoped<IJumioUserCaseService, JumioUserCaseService>();
            services.AddScoped<ITempKeysService, TempKeysService>();
            services.AddScoped<IExtractImageService, ExtractImageService>();

            return services;
        }

    }
}
