using Microsoft.Extensions.DependencyInjection;
using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using System.Reflection;

namespace Process.Application.UseCases.ServicesUseCases
{
    public class AntiSpoofingStrategyService<T> : IAntiSpoofingStrategyService<T>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _strategyTypes;

        public AntiSpoofingStrategyService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _strategyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => new
                {
                    Type = t,
                    Attribute = t.GetCustomAttribute<AntiSpoofingAttribute>()
                })
                .Where(x => x.Attribute != null)
                .ToDictionary(x => x.Attribute!.Type, x => x.Type, StringComparer.OrdinalIgnoreCase);
        }

        public T Resolve(string type)
        {
            if (_strategyTypes.TryGetValue(type, out var implType))
            {
                return (T)_serviceProvider.GetRequiredService(implType);
            }

            throw new NotSupportedException($"No existe una estrategia registrada para '{type}'.");
        }
    }
}
