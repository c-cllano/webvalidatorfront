namespace Process.Domain.Entities
{
    public class Permission
    {
        public long PermissionId { get; set; }
        public long MenuId { get; set; }
        public string Code { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty; 
        public int Order { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
