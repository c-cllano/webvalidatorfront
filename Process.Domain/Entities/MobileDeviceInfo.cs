namespace Process.Domain.Entities
{
    public class MobileDeviceInfo
    {
        public long MobileDeviceInfoId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string MediaDeviceInfo { get; set; } = string.Empty;
        public string? BestCameraManual { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public string OS { get; set; } = string.Empty;
        public string OSVersion { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
    }
}
