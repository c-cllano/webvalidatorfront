namespace Process.Domain.Entities
{
    public class RoleMenu
    {
        public long RoleMenuId { get; set; }
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long CreatorUserId { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
