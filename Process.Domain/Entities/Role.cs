namespace Process.Domain.Entities
{
    public class Role
    { 
        public long RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long ClientId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatorUserId { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Users { get; set; }
        public long ClientId { get; set; }
    }
}
