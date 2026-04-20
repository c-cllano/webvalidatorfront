using MediatR;
using Process.Domain.Repositories;

namespace Process.Application.DeviceInfo.GetMobileDeviceInfo
{
    public class GetMobileDeviceInfoQueryHandler(
        IMobileDeviceInfoRepository mobileDeviceInfoRepository
    ) : IRequestHandler<GetMobileDeviceInfoQuery, GetMobileDeviceInfoResponse>
    {
        private readonly IMobileDeviceInfoRepository _mobileDeviceInfoRepository = mobileDeviceInfoRepository;

        public async Task<GetMobileDeviceInfoResponse> Handle(GetMobileDeviceInfoQuery request, CancellationToken cancellationToken)
        {
            var mobileDeviceInfo = await _mobileDeviceInfoRepository
                .GetMobileDeviceInfoByModelAsync(request.Model, request.OS, request.OSVersion, request.Browser);

            if (mobileDeviceInfo == null)
                return new();

            return new()
            {
                MobileDeviceInfoId = mobileDeviceInfo!.MobileDeviceInfoId,
                Brand = mobileDeviceInfo.Brand,
                Model = mobileDeviceInfo.Model,
                MediaDeviceInfo = mobileDeviceInfo.MediaDeviceInfo,
                BestCameraManual = mobileDeviceInfo.BestCameraManual,
                OS = mobileDeviceInfo.OS,
                OSVersion = mobileDeviceInfo.OSVersion,
                Browser = mobileDeviceInfo.Browser
            };
        }
    }
}
