namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IPlatformConnectionStrategyService<T>
    {
        T Resolve(string platformConnectionType);
    }
}
