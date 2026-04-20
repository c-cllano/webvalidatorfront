using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IMobileDeviceInfoRepository
    {
        Task<MobileDeviceInfo?> GetMobileDeviceInfoByModelAsync(string model, string os, string osVersion, string browser);
        Task<MobileDeviceInfo> CreateUpdateMobileDeviceInfoAsync(MobileDeviceInfo mobileDeviceInfo);
    }
}
