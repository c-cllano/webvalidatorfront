namespace Process.Domain.Entities
{
    public class ValidationProcessDeviceInfo
    {
        public long ValidationProcessDeviceInfoId { get; set; }
        public long ValidationProcessId { get; set; }
        public long MobileDeviceInfoId { get; set; }
        public string? BestCameraAutomatic { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
