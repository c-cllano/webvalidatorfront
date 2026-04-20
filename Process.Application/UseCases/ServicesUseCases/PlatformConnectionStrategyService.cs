using Microsoft.Extensions.DependencyInjection;
using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using System.Reflection;

namespace Process.Application.UseCases.ServicesUseCases
{
    public class PlatformConnectionStrategyService<T> : IPlatformConnectionStrategyService<T>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _strategyTypes;

        public PlatformConnectionStrategyService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _strategyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => new
                {
                    Type = t,
                    Attribute = t.GetCustomAttribute<PlatformConnectionAttribute>()
                })
                .Where(x => x.Attribute != null)
                .ToDictionary(x => x.Attribute!.PlatformConnectionType, x => x.Type, StringComparer.OrdinalIgnoreCase);
        }

        public T Resolve(string platformConnectionType)
        {
            if (_strategyTypes.TryGetValue(platformConnectionType, out var implType))
            {
                return (T)_serviceProvider.GetRequiredService(implType);
            }

            throw new NotSupportedException($"No existe una estrategia registrada para '{platformConnectionType}'.");
        }
    }
}
