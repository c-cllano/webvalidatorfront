using MediatR;
using Process.Application.DeviceInfo.GetMobileDeviceInfo;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.DeviceInfo.CreateUpdateMobileDeviceInfo
{
    public class CreateUpdateMobileDeviceInfoCommandHandler(
        IMobileDeviceInfoRepository mobileDeviceInfoRepository,
        IValidationProcessDeviceInfoRepository validationProcessDeviceInfoRepository
    ) : IRequestHandler<CreateUpdateMobileDeviceInfoCommand, GetMobileDeviceInfoResponse>
    {
        private readonly IMobileDeviceInfoRepository _mobileDeviceInfoRepository = mobileDeviceInfoRepository;
        private readonly IValidationProcessDeviceInfoRepository _validationProcessDeviceInfoRepository = validationProcessDeviceInfoRepository;

        public async Task<GetMobileDeviceInfoResponse> Handle(CreateUpdateMobileDeviceInfoCommand request, CancellationToken cancellationToken)
        {
            MobileDeviceInfo mobileDeviceInfo = new()
            {
                Brand = request.Brand,
                Model = request.Model,
                MediaDeviceInfo = request.MediaDeviceInfo,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                Active = true,
                IsDeleted = false,
                OS = request.OS,
                OSVersion = request.OSVersion,
                Browser = request.Browser
            };

            mobileDeviceInfo = await _mobileDeviceInfoRepository
                .CreateUpdateMobileDeviceInfoAsync(mobileDeviceInfo);

            ValidationProcessDeviceInfo validationProcessDeviceInfo = new()
            {
                ValidationProcessId = request.ValidationProcessId,
                MobileDeviceInfoId = mobileDeviceInfo.MobileDeviceInfoId,
                BestCameraAutomatic = request.BestCameraAutomatic,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                Active = true,
                IsDeleted = false
            };

            await _validationProcessDeviceInfoRepository
                .SaveValidationProcessDeviceInfoAsync(validationProcessDeviceInfo);

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
