namespace Process.Domain.Entities
{
    public class RolePermission
    {
        public long RolePermissionId { get; set; }
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long CreatorUserId { get; set; }
    }
}
