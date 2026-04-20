using MediatR;

namespace Process.Application.DeviceInfo.GetMobileDeviceInfo
{
    public record GetMobileDeviceInfoQuery(
        string Model,
        string OS,
        string OSVersion,
        string Browser
    ) : IRequest<GetMobileDeviceInfoResponse>;
}
