namespace Process.Application.Roles
{
    public class RoleResponse
    {
        public long RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long ClientId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatorUserId { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public List<long> Permissions { get; set; } = new();
    }
}
