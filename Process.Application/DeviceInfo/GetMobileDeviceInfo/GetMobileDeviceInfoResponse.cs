namespace Process.Application.DeviceInfo.GetMobileDeviceInfo
{
    public class GetMobileDeviceInfoResponse
    {
        public long MobileDeviceInfoId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string MediaDeviceInfo { get; set; } = string.Empty;
        public string OS { get; set; } = string.Empty;
        public string OSVersion { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
        public string? BestCameraManual { get; set; }
    }
}
