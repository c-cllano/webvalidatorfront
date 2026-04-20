using Process.Domain.Parameters.MonitoringDataDog;

namespace Process.Domain.Repositories
{
    public interface IMonitoringDataDogImg
    {
        Task<IEnumerable<MonitoringDataDogResponse>> GetMonitoringDataDogImg(IEnumerable<string> parameterNames);
    }
}
