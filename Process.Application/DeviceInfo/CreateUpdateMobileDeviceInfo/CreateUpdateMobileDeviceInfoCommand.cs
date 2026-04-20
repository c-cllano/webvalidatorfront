using MediatR;
using Process.Application.DeviceInfo.GetMobileDeviceInfo;

namespace Process.Application.DeviceInfo.CreateUpdateMobileDeviceInfo
{
    public record CreateUpdateMobileDeviceInfoCommand(
        string Brand,
        string Model,
        string OS,
        string OSVersion,
        string Browser,
        string MediaDeviceInfo,
        long ValidationProcessId,
        string? BestCameraAutomatic = null
    ) : IRequest<GetMobileDeviceInfoResponse>;
}
