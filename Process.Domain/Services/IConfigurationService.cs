namespace Process.Domain.Services
{
    public interface IConfigurationService
    {
        string GetConfiguration(string key);
    }
}
