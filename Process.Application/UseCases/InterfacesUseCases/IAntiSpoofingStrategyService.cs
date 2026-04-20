namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IAntiSpoofingStrategyService<T>
    {
        T Resolve(string type);
    }
}
