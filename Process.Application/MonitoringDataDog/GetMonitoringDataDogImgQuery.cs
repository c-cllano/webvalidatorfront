using MediatR;

namespace Process.Application.MonitoringDataDog
{
    public record GetMonitoringDataDogImgQuery(string Type) : IRequest<GetMonitoringDataDogImgResponse>;
}
