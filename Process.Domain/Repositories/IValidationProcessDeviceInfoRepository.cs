using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IValidationProcessDeviceInfoRepository
    {
        Task<ValidationProcessDeviceInfo> SaveValidationProcessDeviceInfoAsync(ValidationProcessDeviceInfo validationProcessDeviceInfo);
    }
}
